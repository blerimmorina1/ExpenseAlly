import * as signalR from '@microsoft/signalr';

class SignalRService {
    private connection: signalR.HubConnection;

    constructor(hubUrl: string) {
        this.connection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl, {
            accessTokenFactory: () => {
                return localStorage.getItem("auth_token");
            }
        }) // Specify the SignalR hub URL
            .withAutomaticReconnect() // Automatically reconnect if the connection is lost
            .configureLogging(signalR.LogLevel.Information) // Log SignalR activity
            .build();

        // this.connection.on("ReceiveNotification", (notification: { id: string, message: string }) => {
        //     console.log(notification);
        // });
    }

    public async startConnection(): Promise<void> {
        try {
            if (this.connection.state === signalR.HubConnectionState.Disconnected) {
                await this.connection.start();
                console.log('SignalR Connected');
            } else {
                console.log('SignalR is already in a state of ', this.connection.state);
            }
        } catch (err) {
            console.error('SignalR Connection Error: ', err);
            setTimeout(() => this.startConnection(), 5000); // Retry connection after 5 seconds
        }
    }

    public registerNotificationHandler(methodName: string, callback: (...args: any[]) => void): void {
        this.connection.on(methodName, callback);
    }

    public unregisterNotificationHandler(methodName: string): void {
        this.connection.off(methodName);
    }

    public async stopConnection(): Promise<void> {
        if (this.connection.state !== signalR.HubConnectionState.Disconnected) {
            await this.connection.stop();
            console.log('SignalR Connection Stopped');
        }
    }
}

export default SignalRService;
