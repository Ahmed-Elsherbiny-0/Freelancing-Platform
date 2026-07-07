import {
  afterNextRender,
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  effect,
  ElementRef,
  inject,
  NgZone,
  signal,
  viewChild,
  viewChildren,
} from '@angular/core';
import { FooterComponent } from '../../layout/footer/footer.component';
import { HeaderComponent } from '../../layout/header/header.component';
import { HomeFreelancerCardComponent } from './home-freelancer-card/home-freelancer-card.component';
import { AuthService } from '../../core/services/auth.service';
import { ScrollAnimate } from '../../shared/directives/scroll-animate';
import { User } from '../../shared/models/user';

@Component({
  selector: 'app-home',
  imports: [FooterComponent, HeaderComponent, HomeFreelancerCardComponent, ScrollAnimate],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class HomeComponent implements AfterViewInit {
  cdr = inject(ChangeDetectorRef);
  swiperRef = viewChild<ElementRef>('swiper');
  authService = inject(AuthService);
  PageSize = 4;
  swiperBreakpoints = {
    0: {
      slidesPerView: 1,
      slidesPerGroup: 1,
    },
    480: {
      slidesPerView: 2,
      slidesPerGroup: 2,
    },
    768: {
      slidesPerView: 3,
      slidesPerGroup: 3,
    },
    992: {
      slidesPerView: 4,
      slidesPerGroup: 4,
    },
  };
  activeIndex = signal<number>(0);
  private readonly defaultUser: User = {
    id: '1',
    fristName: 'Ahmed',
    lastName: 'Elsherbiny',
    title: 'Backend Developer',
    rate: 5,
    level: 'Expert',
    description: 'I build front-end, back-end, and design systems for websites.',
    salary: 150,
    lastOnline: '5 hours ago',
    pictureUrl: '/computer1.webp',
    email: 'ahmed@test.com',
    permissions: [],
    roles: [],
    refreshToken: '',
    token: '',
    isClient: false,
  };

  list: User[] = [];
  listCount: { id: number }[] = [];
  constructor() {
    this.list = Array.from({ length: 15 }, (_, i) => {
      return {
        ...this.defaultUser,
        id: `${i + 1}`,
      };
    });
    for (let i = 0; i < this.list.length; i += this.PageSize) this.listCount.push({ id: i + 1 });
  }
  // swiper
  get swiper() {
    return this.swiperRef()?.nativeElement.swiper;
  }
  ngAfterViewInit() {
    this.activeIndex.set(0);
  }

  onClickBullets(index: number) {
    this.swiper.slideTo(index * this.PageSize);
    this.activeIndex.set(index);
  }
  swiperRightClick() {
    let res = this.swiper.slideNext();
    if (res) {
      this.activeIndex.set(Math.min((this.activeIndex() + 1) % this.PageSize, this.list.length));
    }
  }
  swiperLeftClick() {
    let res = this.swiper.slidePrev();
    if (res && this.activeIndex() == 0) {
      this.activeIndex.set(4);
    }
    if (res) {
      this.activeIndex.set(Math.max((this.activeIndex() - 1) % this.PageSize, 0));
    }
    if (this.activeIndex() == 0 && !res) {
      this.activeIndex.set(0);
    }
    console.log(this.activeIndex());
  }
}
