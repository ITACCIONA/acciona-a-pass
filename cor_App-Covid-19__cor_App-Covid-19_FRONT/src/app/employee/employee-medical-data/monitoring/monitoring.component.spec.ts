import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ConfigTestingModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { MastersService } from '../../../masters.service';
import { UiModule } from '../../../ui/ui.module';
import { MonitoringComponent } from './monitoring.component';

class MockMasters {
  analytics = {
    parameters: []
  };
  images = {
    parameters: [{ idParameter: '' }]
  };
}

describe('MonitoringComponent', () => {
  let component: MonitoringComponent;
  let fixture: ComponentFixture<MonitoringComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [MonitoringComponent],
      imports: [
        UiModule,
        MatCardModule,
        TranslateModule,
        I18NextModule,
        HttpClientModule,
        ConfigTestingModule,
        MatListModule,
        MatButtonToggleModule,
        MatFormFieldModule,
        MatDatepickerModule,
        MatIconModule,
        ReactiveFormsModule,
        MatSnackBarModule
      ],
      providers: [{ provide: MastersService, useClass: MockMasters }]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MonitoringComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
