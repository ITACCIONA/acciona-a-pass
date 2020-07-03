import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { combineLatest } from 'rxjs';
import { filter } from 'rxjs/operators';
import { IMasterConfig, MastersService } from '../../../masters.service';
import { ITestResult } from '../../../ui/test-result/test-result.component';
import { ToastrService } from '../../../ui/toastr/toastr.service';
import { EmployeeService } from '../../employee.service';

export const insertAtLeastOne: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
  const touched = Object.entries(control.controls)
    .filter(([key, value]) => key !== 'date')
    .some(([key, field]) => field.value !== null);
  return touched ? null : { noFieldDefined: true };
};

@Component({
  selector: 'app-monitoring',
  templateUrl: './monitoring.component.html',
  styleUrls: ['./monitoring.component.scss']
})
export class MonitoringComponent implements OnInit {
  @ViewChild('imageModal') imageModal: TemplateRef<any>;
  @ViewChild('analyticsModal') analyticsModal: TemplateRef<any>;

  @Input()
  id: string;

  isLoading = true;
  images: ITestResult[];
  analytics: ITestResult[];

  imagesConfig: IMasterConfig;
  analyticsConfig: IMasterConfig;

  imageForm: FormGroup;
  analyticForm: FormGroup;

  constructor(
    private service: EmployeeService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private masters: MastersService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.imageForm = this.fb.group({
      date: null,
      type: null,
      result: [null, Validators.required]
    });

    this.analyticForm = this.fb.group(
      {
        date: [null, Validators.required]
      },
      { validators: insertAtLeastOne }
    );

    combineLatest([this.masters.images, this.masters.analytics])
      .pipe(filter(([i, a]) => !!i && !!a))
      .subscribe(([images, analytics]) => {
        this.imagesConfig = images;
        this.analyticsConfig = analytics;
        this.analyticsConfig.parameters.map(param => {
          this.analyticForm.addControl(param.name, new FormControl(null));
        });
        this.fetch();
        this.dialog.afterAllClosed.subscribe(() => this.resetForm());
      });
  }

  resetForm() {
    this.imageForm.reset({
      date: new Date(),
      type: this.imagesConfig.parameters[0].idParameter,
      result: null
    });

    this.analyticForm.get('date').patchValue(new Date());
    this.analyticsConfig.parameters.map(param => this.analyticForm.get(param.name).patchValue(null));
  }

  fetch() {
    this.resetForm();

    this.service.getMonitoring(this.id).subscribe(data => {
      const img = data.find(d => d.nameParameterType === 'Imagen');
      this.images = img
        ? img.monitoringValue.map(i => ({
            id: i.id,
            date: i.fechaTest,
            name: `MONITORING.${i.parameterValues[0].nameParameter.toUpperCase()}`,
            positive: i.parameterValues[0].value,
            breakdown: [
              {
                name: i.parameterValues[0].value ? 'TESTS.POSITIVE' : 'TESTS.NEGATIVE',
                positive: i.parameterValues[0].value
              }
            ]
          }))
        : [];
      this.images.sort((a, b) => (new Date(b.date) as any) - (new Date(a.date) as any));
      const ana = data.find(d => d.nameParameterType === 'Analitica');
      this.analytics = ana
        ? ana.monitoringValue.map(i => {
            const positive = i.parameterValues.some(v => v.value === true);

            const breakdown = i.parameterValues.map(v => ({
              name: `MONITORING.${v.nameParameter.toUpperCase()}`,
              positive: v.value
            }));

            return {
              id: i.id,
              date: i.fechaTest,
              positive,
              breakdown
            };
          })
        : [];

      this.analytics.sort((a, b) => (new Date(b.date) as any) - (new Date(a.date) as any));

      this.isLoading = false;
    });
  }

  openModal(type: 'image' | 'analytics' = 'image') {
    const modalConf = { width: '525px' };
    type === 'image' ? this.dialog.open(this.imageModal, modalConf) : this.dialog.open(this.analyticsModal, modalConf);
  }

  addImage() {
    this.isLoading = true;
    const form = this.imageForm.value;

    const params = [{ idParameter: form.type, value: form.result }];
    this.service.addMonitoring(this.id, form.date, this.imagesConfig.idParameterType, params).subscribe(
      () => {
        this.isLoading = false;
        this.toastr.success('COMMONS.SUCCESS');
        this.fetch();
        this.dialog.closeAll();
      },
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
      }
    );
  }

  addAnalytic() {
    this.isLoading = true;
    const form = this.analyticForm.value;

    const params = this.analyticsConfig.parameters
      .map(param => ({
        idParameter: param.idParameter,
        value: form[param.name]
      }))
      .filter(v => v.value !== null);

    this.service.addMonitoring(this.id, form.date, this.analyticsConfig.idParameterType, params).subscribe(
      () => {
        this.isLoading = false;
        this.toastr.success('COMMONS.SUCCESS');
        this.fetch();
        this.dialog.closeAll();
      },
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
      }
    );
  }

  getI18nLabel(key: string) {
    return `MONITORING.${key.toUpperCase()}`;
  }
}
