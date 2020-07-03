import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { forkJoin, Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { ICountry, IDivision, ILocation, IPassportState, MastersService } from '../../masters.service';
import { ToastrService } from '../../ui/toastr/toastr.service';
import { NotificationsService } from '../notifications.service';

@Component({
  selector: 'app-general-form',
  templateUrl: './general-form.component.html',
  styleUrls: ['./general-form.component.scss']
})
export class GeneralFormComponent implements OnInit {
  @Output()
  sentSuccess: EventEmitter<void> = new EventEmitter();

  @Output()
  sentError: EventEmitter<void> = new EventEmitter();

  isLoading = true;

  form: FormGroup;

  countries: ICountry[] = [];
  filteredCountries: Observable<ICountry[]>;

  divisions: IDivision[] = [];
  locations: ILocation[] = [];
  filteredWorkcenters: Observable<ILocation[]>;

  statuses: IPassportState[] = [];

  constructor(private masters: MastersService, private service: NotificationsService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      target: new FormControl('all'),
      country: new FormControl(null, this.conditionalRequired('country')),
      division: new FormControl(null, this.conditionalRequired('division')),
      workcenter: new FormControl(null, this.conditionalRequired('workcenter')),
      interacciona: new FormControl(false),
      status: new FormControl(0),
      title: new FormControl('', Validators.required),
      comment: new FormControl('', Validators.required)
    });

    forkJoin([
      this.masters.fetchCountries(),
      this.masters.fetchDivisions(),
      this.masters.fetchLocations(),
      this.masters.fetchStatuses()
    ]).subscribe(([countries, divisions, locations, statuses]) => {
      this.countries = countries;
      this.divisions = divisions;
      this.statuses = statuses;
      this.locations = locations;
      this.isLoading = false;
    });

    this.form.get('target').valueChanges.subscribe(() => {
      this.form.get('country').reset();
      this.form.get('division').reset();
      this.form.get('workcenter').reset();
    });

    this.filteredCountries = this.form.get('country').valueChanges.pipe(
      startWith(''),
      map(value => this._filter<ICountry>(value, this.countries))
    );

    this.filteredWorkcenters = this.form.get('workcenter').valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value?.name)),
      map(value => this._filter<ILocation>(value, this.locations))
    );
  }

  private _filter<T = any>(value: string, list: any[]): T[] {
    const filterValue = value?.toLowerCase();
    return list.filter(option => option.name.toLowerCase().includes(filterValue));
  }

  displayFn(location): string {
    return location && location.name ? location.name : '';
  }

  trackById(index: number, item: ILocation) {
    return item.idLocation;
  }

  isValid() {
    return this.form.valid;
  }

  send() {
    this.isLoading = true;
    const form = this.form.value;

    let method;
    switch (form.target) {
      case 'all':
        method = this.service.sendToAll(form);
        break;
      case 'country':
        method = this.service.sendToCountry(form);
        break;
      case 'division':
        method = this.service.sendToDivision(form);
        break;
      case 'workcenter':
        method = this.service.sendToLocation({ ...form, workcenter: form.workcenter.idLocation });
        break;
    }

    method.subscribe(
      () => {
        this.toastr.success('NOTIFICATIONS.SUCCESS');
        this.isLoading = false;
        this.form.reset();
        this.sentSuccess.emit();
      },
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
        this.sentError.emit();
      }
    );
  }

  private conditionalRequired(field: string): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      if (control.parent) {
        const target = control.parent.get('target').value;
        if (target === 'country' && field === 'country' && !control.value) {
          return { wrongField: true };
        } else if (target === 'division' && field === 'division' && !control.value) {
          return { wrongField: true };
        } else if (target === 'workcenter' && field === 'workcenter' && !control.value) {
          return { wrongField: true };
        } else {
          return null;
        }
      }
      return null;
    };
  }
}
