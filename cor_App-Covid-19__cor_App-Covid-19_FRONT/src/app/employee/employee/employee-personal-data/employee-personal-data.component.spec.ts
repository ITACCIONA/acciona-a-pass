import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { ConfigTestingModule, PipesModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from '../../../ui/ui.module';
import { EmployeePersonalDataComponent } from './employee-personal-data.component';

describe('EmployeePersonalDataComponent', () => {
  let component: EmployeePersonalDataComponent;
  let fixture: ComponentFixture<EmployeePersonalDataComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EmployeePersonalDataComponent],
      imports: [
        UiModule,
        TranslateModule,
        I18NextModule,
        MatCardModule,
        MatListModule,
        MatIconModule,
        PipesModule,
        MatDividerModule,
        HttpClientModule,
        ConfigTestingModule,
        MatDialogModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePersonalDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
