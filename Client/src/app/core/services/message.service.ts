import { EventEmitter, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { ToastService } from './toast.service';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { User } from '../../shared/models/user';
import { Message } from '../../shared/models/message';
import { BehaviorSubject } from 'rxjs';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private hubsUrl = environment.hupsUrl;
  private hubConnection: HubConnection | null = null;
  private presence = inject(PresenceService);
  messageThread = signal<Message[]>([]);
  sendMessageEvent = new EventEmitter<boolean>();

  async createHubConnection(user: User, otherUsername: string) {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      return;
    }
    if (this.hubConnection) {
      await this.hubConnection.stop();
    }
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubsUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveMessageThread', (messages: Message[]) => {
      this.messageThread.set(messages);

      const unreaded = this.presence
        .unReadedMessages()
        .find(
          (x) =>
            x.username == messages[0].recipientUsername || x.username == messages[0].senderUsername,
        );
      if (unreaded) {
        this.presence.unReadedMessages.update((m) =>
          m.map((x) =>
            x.username === messages[0].recipientUsername ||
            x.username === messages[0].senderUsername
              ? { ...x, count: 0 }
              : x,
          ),
        );
      }
    });

    this.hubConnection.on('NewMessage', (message) => {
      this.messageThread.update((messages) => [...messages, message]);
    });
    await this.hubConnection.start().catch((err) => console.log(err));
  }

  async stopHubConnection() {
    if (
      this.hubConnection &&
      this.hubConnection.state !== HubConnectionState.Disconnected &&
      this.hubConnection.state !== HubConnectionState.Disconnecting
    ) {
      await this.hubConnection.stop().catch((err) => console.log(err));
      this.hubConnection = null;
    }
  }
  async sendMessage(username: string, content: string) {
    return this.hubConnection?.invoke('SendMessage', {
      RecipientUsername: username,
      Content: content,
    });
  }
}
