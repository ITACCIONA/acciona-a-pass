import { registerLocaleData } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import localeEs from '@angular/common/locales/es';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ConfigService, PipesModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { LoadingComponent } from '../loading/loading.component';
import { ToastrService } from '../toastr/toastr.service';
import { TestResultComponent } from './test-result.component';

describe('TestResultComponent', () => {
  let component: TestResultComponent;
  let fixture: ComponentFixture<TestResultComponent>;
  registerLocaleData(localeEs);

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        PipesModule,
        MatIconModule,
        MatMenuModule,
        MatDialogModule,
        HttpClientModule,
        MatSnackBarModule,
        MatProgressSpinnerModule,
        I18NextModule.forRoot()
      ],
      declarations: [TestResultComponent, LoadingComponent],
      providers: [ConfigService, ToastrService]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestResultComponent);
    component = fixture.componentInstance;
    component.data = { name: 'a', date: '2020-04-19T08:00:00+02:00', breakdown: [], positive: false };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
