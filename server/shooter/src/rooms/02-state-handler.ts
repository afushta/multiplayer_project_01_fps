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

    @type("number")
    angularVelocity = 0;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    createPlayer(sessionId: string) {
        this.players.set(sessionId, new Player());
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, movement: any) {
        var player = this.players.get(sessionId);
        player.position.updateValue(movement.position);
        player.velocity.updateValue(movement.velocity);
        player.rotation.updateValue(movement.rotation);
        if (movement.angularVelocity != null) player.angularVelocity = movement.angularVelocity;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            // console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            this.state.movePlayer(client.sessionId, data);
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client) {
        this.state.createPlayer(client.sessionId);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
