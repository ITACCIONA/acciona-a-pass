import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ConfigTestingModule, PipesModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { MastersService } from '../../masters.service';
import { UiModule } from '../../ui/ui.module';
import { EmployeeMedicalDataComponent } from './employee-medical-data.component';
import { FeverRecordsComponent } from './fever-records/fever-records.component';
import { MonitoringComponent } from './monitoring/monitoring.component';
import { PassportWarningComponent } from './passport-warning/passport-warning.component';
import { PassportComponent } from './passport/passport.component';
import { ResponsibleStatementComponent } from './responsible-statement/responsible-statement.component';
import { RiskFactorsComponent } from './risk-factors/risk-factors.component';
import { TestsComponent } from './tests/tests.component';

class MockMasters {
  analytics = {
    parameters: []
  };
  images = {
    parameters: [{ idParameter: '' }]
  };
  getRole() {
    return '';
  }
}

describe('EmployeeMedicalDataComponent', () => {
  let component: EmployeeMedicalDataComponent;
  let fixture: ComponentFixture<EmployeeMedicalDataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        EmployeeMedicalDataComponent,
        PassportComponent,
        PassportWarningComponent,
        RiskFactorsComponent,
        TestsComponent,
        MonitoringComponent,
        FeverRecordsComponent,
        ResponsibleStatementComponent
      ],
      imports: [
        TranslateModule,
        I18NextModule,
        MatCardModule,
        MatDividerModule,
        PipesModule,
        MatIconModule,
        MatListModule,
        MatDialogModule,
        UiModule,
        HttpClientModule,
        ConfigTestingModule,
        MatDatepickerModule,
        MatFormFieldModule,
        ReactiveFormsModule,
        MatButtonToggleModule,
        MatSnackBarModule
      ],
      providers: [{ provide: MastersService, useClass: MockMasters }]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeMedicalDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
