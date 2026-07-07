import { Directive, ElementRef, inject, OnInit } from '@angular/core';

@Directive({
  selector: '[appScrollAnimate]',
})
export class ScrollAnimate implements OnInit {
  elementRef = inject(ElementRef);

  ngOnInit() {
    const element = this.elementRef.nativeElement;
    element.classList.add('scroll-hidden');

    let obsCallBack = function (
      entries: IntersectionObserverEntry[],
      observer: IntersectionObserver,
    ) {
      entries.forEach((ele) => {
        if (ele.isIntersecting) {
          element.classList.add('scroll-visible');
          observer.unobserve(element);
        }
      });
    };

    const observer = new IntersectionObserver(obsCallBack, {
      root: null,
      threshold: 0.15,
    });

    observer.observe(element);
  }
}
