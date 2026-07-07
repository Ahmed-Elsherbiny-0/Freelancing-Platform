import { Component, computed, inject, Signal, signal } from '@angular/core';
import { HeaderComponent } from '../../layout/header/header.component';
import { FooterComponent } from '../../layout/footer/footer.component';
import { MessageChatComponent } from './message-chat/message-chat.component';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { PresenceService } from '../../core/services/presence.service';
import { MessageService } from '../../core/services/message.service';
import { AuthService } from '../../core/services/auth.service';
import { DatePipe } from '../../shared/pipes/date-pipe';

@Component({
  selector: 'app-messages',
  imports: [HeaderComponent, FooterComponent, MessageChatComponent, RouterOutlet, DatePipe],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css',
})
export class MessagesComponent {
  router = inject(Router);
  route = inject(ActivatedRoute);
  presence = inject(PresenceService);
  messageService = inject(MessageService);
  authService = inject(AuthService);
  senderSendMessage = signal<boolean>(false);

  arr = computed(
    (): {
      name: string | null;
      img: string;
      lastMessage: Signal<string | undefined>;
      lastvisit: Signal<string>;
      username: string | null;
      unreaded: Signal<number | undefined>;
    }[] => {
      return (this.presence.userFrinds() ?? []).map((friend) => ({
        name: friend.fullName,
        img: friend.url,
        username: friend.otherUsername,
        lastMessage: computed(() => {
          const received = this.presence.receiveMessage();
          if (
            received?.content != null &&
            (received.otherUsername == friend.otherUsername ||
              received.otherUsername == this.authService.currentUser()?.email)
          ) {
            friend.content = received.content;
            return received.content;
          }
          return friend.content;
        }),
        lastvisit: computed(() => {
          return friend.messageSent;
        }),
        unreaded: computed(() => {
          if (this.senderSendMessage()) return 0;
          const received = this.presence.receiveMessage();
          if (received?.content != null && received.otherUsername == friend.otherUsername) {
            return received.countUnRead;
          }
          return (
            this.presence.unReadedMessages().find((x) => x.username === friend.otherUsername)
              ?.count ?? 0
          );
        }),
      }));
    },
  );

  openChat = signal(false);

  message = signal<{ img: string }>({
    img: 'computer1.webp',
  });

  ngOnInit() {
    console.log('hi');
    this.openChat.set(false);
    this.messageService.sendMessageEvent.subscribe((msg) => {
      this.senderSendMessage.set(true);
    });

    this.route.firstChild?.paramMap.subscribe(async (x) => {
      if (x.get('username') != null && x.get('username') != '') {
        this.openChat.set(true);
        await this.onOpenChat(x.get('username'));
      }
    });
  }

  async onOpenChat(username: string | null) {
    if (!this.openChat()) this.openChat.set(!this.openChat());
    if (this.authService.currentUser() && username) {
      await this.messageService.stopHubConnection();
      await this.messageService.createHubConnection(this.authService.currentUser()!, username);
    }
    this.router.navigate(['/messages', username]);
  }
}
