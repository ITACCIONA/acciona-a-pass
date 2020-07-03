import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { ConfigTestingModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../../../ui/ui.module';
import { RiskFactorsComponent } from './risk-factors.component';

describe('RiskFactorsComponent', () => {
  let component: RiskFactorsComponent;
  let fixture: ComponentFixture<RiskFactorsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RiskFactorsComponent],
      imports: [
        UiModule,
        MatListModule,
        MatIconModule,
        ConfigTestingModule,
        HttpClientModule,
        TranslateModule,
        I18NextModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RiskFactorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
