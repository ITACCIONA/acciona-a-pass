import { TestBed } from '@angular/core/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { PipesModule } from '@commons';
import { I18NextModule } from 'angular-i18next';
import { ToastrService } from './toastr.service';

describe('ToastrService', () => {
  let service: ToastrService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [PipesModule, MatSnackBarModule, I18NextModule.forRoot()]
    });
    service = TestBed.inject(ToastrService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
