import { Component, ComponentFactoryResolver, OnDestroy, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';
import { SidenavService } from './sidenav.service';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit, OnDestroy {
  @ViewChild('sidenav') sidenav: MatSidenav;
  @ViewChild('dynamic', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
  private sub$: Subscription;

  constructor(private service: SidenavService, private factoryResolver: ComponentFactoryResolver) {}

  ngOnInit(): void {
    this.sub$ = this.service.status.subscribe(s => {
      if (this.sidenav) {
        s ? this.sidenav.open() : this.sidenav.close();
      }
    });

    this.service.content.pipe(filter(v => !!v)).subscribe(c => this.addComponent(c));
  }

  change(newStatus: boolean) {
    this.service.set(newStatus);
  }

  close() {
    this.service.close();
  }

  addComponent(component: any) {
    const factory = this.factoryResolver.resolveComponentFactory(component);
    const rendered = factory.create(this.viewContainerRef.parentInjector); // FIXME: check this deprecation
    this.viewContainerRef.insert(rendered.hostView);
  }

  ngOnDestroy() {
    this.sub$.unsubscribe();
  }
}
