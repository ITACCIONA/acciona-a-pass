import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { BroadcastService, MsalService } from '@azure/msal-angular';
import { getAvatarFromName } from '@commons';
import { filter, map, mergeMap } from 'rxjs/operators';
import { MastersService } from './masters.service';
import { SendNotificationGeneralComponent } from './notifications/send-notification-general/send-notification-general.component';
import { HeaderService } from './ui/header/header.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  isIframe: boolean;
  appReady = false;
  error = false;
  avatarName: string;

  constructor(
    private auth: MsalService,
    private masters: MastersService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private header: HeaderService,
    private dialog: MatDialog,
    private iconRegistry: MatIconRegistry,
    private sanitizer: DomSanitizer,
    private broadcastService: BroadcastService
  ) {
    this.isIframe = window !== window.parent && !window.opener;

    this.auth.handleRedirectCallback(() => {});
    this.masters.login();

    this.masters.ready.subscribe(status => {
      if (status === 'failed') {
        this.error = true;
      } else if (status) {
        this.appReady = true;
      }
    });

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        map(() => this.activatedRoute),
        map((route: any) => {
          while (route.firstChild) {
            route = route.firstChild;
          }
          return route;
        }),
        mergeMap((route: any) => route.data)
      )
      .subscribe(rd => this.header.refresh(rd));
    this.registerIcons();

    this.broadcastService.subscribe('msal:acquireTokenSuccess', payload => {
      this.avatarName = getAvatarFromName(payload?.idTokenClaims?.name || '');
    });
  }

  openNotifications() {
    this.dialog.open(SendNotificationGeneralComponent, {
      width: '620px'
    });
  }

  registerIcons() {
    const iconList = [
      'covid-19',
      'medical',
      'risk',
      'thermometer',
      'test',
      'ico-passport',
      'check',
      'cancel',
      'bell',
      'all',
      'country',
      'division',
      'workcenter'
    ];

    iconList.map(icon => {
      this.iconRegistry.addSvgIcon(icon, this.sanitizer.bypassSecurityTrustResourceUrl(`assets/icons/${icon}.svg`));
    });
  }

  retry() {
    localStorage.clear();
    window.history.go(0);
  }
}
