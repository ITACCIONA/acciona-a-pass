import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { RouterTestingModule } from '@angular/router/testing';
import { ConfigTestingModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { MastersService } from 'src/app/masters.service';
import { AvatarComponent } from '../avatar/avatar.component';
import { BreadcrumbsComponent } from '../breadcrumbs/breadcrumbs.component';
import { HeaderNotificationsComponent } from '../header-notifications/header-notifications.component';
import { HeaderComponent } from './header.component';

class MockMasters {
  analytics = {
    parameters: []
  };
  getRole = () => {};
  roleName = ['PRL'];
  images = {
    parameters: [{ idParameter: '' }]
  };
}

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [HeaderComponent, AvatarComponent, BreadcrumbsComponent, HeaderNotificationsComponent],
      providers: [{ provide: MastersService, useClass: MockMasters }],
      imports: [
        RouterTestingModule,
        TranslateModule,
        I18NextModule,
        MatIconModule,
        MatButtonModule,
        HttpClientModule,
        ConfigTestingModule,
        MatSelectModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
