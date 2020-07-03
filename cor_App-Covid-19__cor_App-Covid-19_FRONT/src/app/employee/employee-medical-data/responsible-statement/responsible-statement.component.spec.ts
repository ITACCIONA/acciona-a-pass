import { HttpClientModule } from '@angular/common/http';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterTestingModule } from '@angular/router/testing';
import { ConfigTestingModule, PipesModule, TranslateModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { UiModule } from 'src/app/ui/ui.module';
import { EmployeeModule } from '../../employee.module';
import { ResponsibleStatementComponent } from './responsible-statement.component';

describe('ResponsibleStatementComponent', () => {
  let component: ResponsibleStatementComponent;
  let fixture: ComponentFixture<ResponsibleStatementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        TranslateModule,
        I18NextModule,
        RouterTestingModule,
        MatIconModule,
        MatGridListModule,
        ReactiveFormsModule,
        MatInputModule,
        EmployeeModule,
        UiModule,
        MatTableModule,
        ConfigTestingModule,
        HttpClientModule,
        PipesModule
      ]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResponsibleStatementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
