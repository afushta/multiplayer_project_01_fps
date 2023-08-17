import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Vector3_NO extends Schema {
    @type("number")
    x = 0;

    @type("number")
    y = 0;

    @type("number")
    z = 0;

    constructor (x = 0, y = 0, z = 0)
    {
        super();
        this.x = x;
        this.y = y;
        this.z = z;
    }

    updateValue(newValue: any) {
        if (!newValue) return;

        if (newValue.x != null) this.x = newValue.x;
        if (newValue.y != null) this.y = newValue.y;
        if (newValue.z != null) this.z = newValue.z;
    }
}

export class Player extends Schema {
    @type(Vector3_NO)
    position = new Vector3_NO(Math.random() * 30 - 15, 0, Math.random() * 30 - 15);

    @type(Vector3_NO)
    velocity = new Vector3_NO();

    @type(Vector3_NO)
    rotation = new Vector3_NO();

    @type(Vector3_NO)
    angularVelocity = new Vector3_NO();

    @type("number")
    speed = 0;

    @type("int8")
    maxHP = 0;

    @type("int8")
    currentHP = 0;

    @type("boolean")
    isCrouching = false;

    @type("uint8")
    gun = 0;

    @type("uint8")
    kills = 0;

    @type("uint8")
    deaths = 0;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    createPlayer(sessionId: string, data: any) {
        const player = new Player();
        player.maxHP = data.hp;
        player.currentHP = data.hp;
        player.speed = data.speed;
        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) {
        const player = this.players.get(sessionId);
        player.position.updateValue(movement.position);
        player.velocity.updateValue(movement.velocity);
        player.rotation.updateValue(movement.rotation);
        player.angularVelocity.updateValue(movement.angularVelocity);
        if (movement.isCrouching != null) player.isCrouching = movement.isCrouching;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("shoot", (client, data) => {
            this.broadcast("shoot", data, {except: client});
        });

        this.onMessage("gun", (client, data) => {
            const player = this.state.players.get(client.sessionId);
            player.gun = data.gunId;
        });

        this.onMessage("damage", (client, data) => {
            const clientId = data.id;
            const player = this.state.players.get(clientId);
            player.currentHP -= data.value;

            if (player.currentHP <= 0) {
                this.state.players.get(client.sessionId).kills += 1;
                player.currentHP = player.maxHP;
                player.deaths += 1;
                this.respawnClient(clientId);
            }
        });
    }

    respawnClient (clientId) {
        for (let index = 0; index < this.clients.length; index++) {
            const client = this.clients[index];
            if (client.sessionId != clientId) continue;
            
            const newPosition = new Vector3_NO(Math.random() * 30 - 15, 0, Math.random() * 30 - 15);
            const message = JSON.stringify(newPosition);
            client.send("respawn", message);
            break;
        }
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, options: any) {
        this.state.createPlayer(client.sessionId, options);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
