import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { forkJoin, Observable } from 'rxjs';
import { filter, map, startWith, switchMap } from 'rxjs/operators';
import { IDivision, ILocation, ISuperior, MastersService } from '../../masters.service';
import { ToastrService } from '../../ui/toastr/toastr.service';
import { OutsourcingService } from '../outsourcing.service';

@Component({
  selector: 'app-new-user',
  templateUrl: './new-user.component.html',
  styleUrls: ['./new-user.component.scss']
})
export class NewUserComponent implements OnInit {
  form: FormGroup;
  isLoading = true;

  divisions: IDivision[] = [];
  locations: ILocation[] = [];
  filteredWorkcenters: Observable<ILocation[]>;

  filteredSuperiors: Observable<ISuperior[]>;

  constructor(private masters: MastersService, private service: OutsourcingService, private toastr: ToastrService) {}

  ngOnInit(): void {
    forkJoin([this.masters.fetchDivisions(), this.masters.fetchLocations()]).subscribe(([divisions, locations]) => {
      this.divisions = divisions;
      this.locations = locations;
      this.isLoading = false;
    });

    this.form = new FormGroup({
      name: new FormControl('', Validators.required),
      surname: new FormControl('', Validators.required),
      surname2: new FormControl(''),
      dni: new FormControl(null, Validators.required),
      division: new FormControl(null),
      workcenter: new FormControl(null, Validators.required),
      superior: new FormControl('', Validators.required)
    });
    this.reset();

    this.filteredWorkcenters = this.form.get('workcenter').valueChanges.pipe(
      startWith(''),
      map(value => (typeof value === 'string' ? value : value?.name)),
      map(value => this._filter<ILocation>(value, this.locations))
    );

    this.filteredSuperiors = this.form.get('superior').valueChanges.pipe(
      startWith(''),
      filter(v => v?.length >= 3),
      map(value => (typeof value === 'string' ? value : value?.name)),
      switchMap(value => this.masters.searchEmployees(value))
    );
  }

  private _filter<T = any>(value: string, list: any[]): T[] {
    const filterValue = value && value.toLowerCase();
    return list.filter(option => option.name.toLowerCase().includes(filterValue));
  }

  displaySuperiors(superior) {
    return superior?.completeName;
  }

  displayFn(location): string {
    return location && location.name ? location.name : '';
  }

  trackById(index: number, item: ILocation) {
    return item.idLocation;
  }

  reset() {
    this.form.reset();
    this.form
      .get('superior')
      .patchValue({ id: this.masters.userInfo.idEmpleado, completeName: this.masters.userInfo.nombreUsuario });
  }

  send() {
    const form = this.form.value;

    const data = {
      dni: form.dni,
      nombre: form.name,
      apellidos: form.surname + (form.surname2 ? ` ${form.surname2}` : ''),
      idDivision: form.division,
      idLocalizacion: form.workcenter?.idLocation,
      idResponsable: form.superior?.id
    };

    this.isLoading = true;
    this.service.createEmployee(data).subscribe(
      () => {
        this.toastr.success('COMMONS.SUCCESS');
        this.reset();
        this.isLoading = false;
      },
      () => {
        this.isLoading = false;
        this.toastr.error('COMMONS.ERROR');
      }
    );
  }
}
