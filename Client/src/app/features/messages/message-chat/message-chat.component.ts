import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  computed,
  DestroyRef,
  effect,
  ElementRef,
  EventEmitter,
  inject,
  input,
  OnDestroy,
  OnInit,
  Signal,
  signal,
  viewChild,
  WritableSignal,
} from '@angular/core';
import { PresenceService } from '../../../core/services/presence.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageService } from '../../../core/services/message.service';
import { AuthService } from '../../../core/services/auth.service';
import { DatePipe } from '../../../shared/pipes/date-pipe';
import { UserService } from '../../../core/services/user.service';
import { SimpleUser } from '../../../shared/models/user';

@Component({
  selector: 'app-message-chat',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './message-chat.component.html',
  styleUrl: './message-chat.component.css',
})
export class MessageChatComponent implements OnInit, OnDestroy {
  userService = inject(UserService);
  presenceService = inject(PresenceService);
  authService = inject(AuthService);
  private router = inject(ActivatedRoute);
  messageService = inject(MessageService);
  cdr = inject(ChangeDetectorRef);
  destoryRef = inject(DestroyRef);
  form = new FormGroup({
    inputText: new FormControl('', [Validators.required, Validators.minLength(1)]),
  });
  otherusername = signal<string | null>(null);
  IsOnline = computed(() => this.presenceService.onlineUsers().includes(this.otherusername()!));
  bottomAnchor = viewChild<ElementRef<HTMLDivElement>>('bottomAnchor');
  otherUserChat = signal<{ name?: string; img?: string } | null>(null);
  ngOnInit(): void {
    var sub = this.router.paramMap.subscribe((x) => {
      if (x.get('username') != null && x.get('username') != '') {
        this.otherusername.set(x.get('username'));
        this.userService.getUser(this.otherusername()!).subscribe((x) => {
          if (x != null) {
            this.otherUserChat.set({ name: x.firstName + ' ' + x.lastName, img: x.photo });
            console.log(this.otherUserChat());
          }
        });
      }
    });
    this.destoryRef.onDestroy(() => sub.unsubscribe());
  }
  constructor() {
    effect(() => {
      const records = this.messageService.messageThread();
      const anchor = this.bottomAnchor();
      if (anchor) {
        this.scrollToBottom();
      }
    });
  }

  currentUserChat = computed(() => ({
    name: this.otherUserChat()?.name,
    img: this.otherUserChat()?.img,
  }));

  onSendMessage() {
    if (this.form.valid && this.otherusername) {
      this.messageService
        .sendMessage(this.otherusername()!, this.form.get('inputText')?.value!)
        .then(() => {
          this.messageService.sendMessageEvent.emit(true);
          this.scrollToBottom();
          this.form.reset();
        });
    }
  }
  private scrollToBottom() {
    this.cdr.detectChanges();
    this.bottomAnchor()?.nativeElement.scrollIntoView({ behavior: 'smooth' });
  }

  async ngOnDestroy() {
    await this.messageService.stopHubConnection();
  }
}
