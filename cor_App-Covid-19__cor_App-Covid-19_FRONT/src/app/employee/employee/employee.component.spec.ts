import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ActivatedRoute } from '@angular/router';
import { ConfigTestingModule, PipesModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { MastersService } from '../../masters.service';
import { UiModule } from '../../ui/ui.module';
import { EmployeeMedicalDataComponent } from '../employee-medical-data/employee-medical-data.component';
import { FeverRecordsComponent } from '../employee-medical-data/fever-records/fever-records.component';
import { MonitoringComponent } from '../employee-medical-data/monitoring/monitoring.component';
import { PassportWarningComponent } from '../employee-medical-data/passport-warning/passport-warning.component';
import { PassportComponent } from '../employee-medical-data/passport/passport.component';
import { ResponsibleStatementComponent } from '../employee-medical-data/responsible-statement/responsible-statement.component';
import { RiskFactorsComponent } from '../employee-medical-data/risk-factors/risk-factors.component';
import { TestsComponent } from '../employee-medical-data/tests/tests.component';
import { PrlComponent } from '../prl/prl.component';
import { EmployeePersonalDataComponent } from './employee-personal-data/employee-personal-data.component';
import { EmployeeComponent } from './employee.component';

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

describe('EmployeeComponent', () => {
  let component: EmployeeComponent;
  let fixture: ComponentFixture<EmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        EmployeeComponent,
        EmployeeMedicalDataComponent,
        PassportComponent,
        PassportWarningComponent,
        RiskFactorsComponent,
        TestsComponent,
        MonitoringComponent,
        FeverRecordsComponent,
        EmployeePersonalDataComponent,
        ResponsibleStatementComponent,
        PrlComponent
      ],
      imports: [
        BrowserAnimationsModule,
        MatTabsModule,
        TranslateModule,
        I18NextModule,
        MatCardModule,
        MatListModule,
        MatIconModule,
        UiModule,
        HttpClientModule,
        MatDialogModule,
        ConfigTestingModule,
        ConfigTestingModule,
        PipesModule,
        MatDatepickerModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatButtonToggleModule,
        MatSnackBarModule
      ],
      providers: [
        { provide: MastersService, useClass: MockMasters },
        {
          provide: ActivatedRoute,
          useValue: {
            snapshot: {
              params: {
                id: '1'
              }
            }
          }
        }
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
