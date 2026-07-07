import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { ToastService } from './toast.service';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { User } from '../../shared/models/user';
import { PrimaryUser } from '../../shared/models/primaryuser';
@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  private hubsUrl = environment.hupsUrl;
  private apiUrl = environment.apiUrl;
  private toast = inject(ToastService);
  private hubConnection?: HubConnection;
  userFrinds = signal<PrimaryUser[] | null>(null);
  onlineUsers = signal<string[]>([]);
  unReadedMessages = signal<{ username: string; count: number }[]>([]);
  receiveMessage = signal<{
    otherUsername: string;
    countUnRead: number;
    content: string | null;
  } | null>(null);
  async createHubConnection(user: User) {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      return;
    }
    if (this.hubConnection) {
      await this.hubConnection.stop();
    }
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubsUrl + 'presence', {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('onConnectUser', (username) => {
      this.toast.normal(`${username} has connected Successfully`);
    });

    this.hubConnection.on('onDisconnectUser', (username) => {
      this.toast.normal(`${username} has disconnected `);
    });

    this.hubConnection.on('GetUserFrinds', (users: PrimaryUser[]) => {
      console.log(users);
      this.userFrinds.set(users);
    });
    this.hubConnection.on('GetOnlineUsers', (users) => {
      this.onlineUsers.set(users);
    });

    this.hubConnection.on('GetUnReadedMessages', (arr) => {
      console.log(arr);
      this.unReadedMessages.set(arr);
    });

    this.hubConnection.on('onReciveNewMessage', (arr) => {
      console.log(arr);
      console.log('rokba--------------');

      this.receiveMessage.set(arr);
    });
    return this.hubConnection.start().catch((err) => console.log(err));
  }
  stopHubConnection() {
    if (this.hubConnection?.state == HubConnectionState.Connected) {
      this.hubConnection.stop().catch((err) => console.log(err));
    }
  }
}
