import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { PassportWarningComponent } from './passport-warning.component';

describe('PassportWarningComponent', () => {
  let component: PassportWarningComponent;
  let fixture: ComponentFixture<PassportWarningComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PassportWarningComponent],
      imports: [MatCardModule, MatIconModule, TranslateModule, I18NextModule]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PassportWarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
