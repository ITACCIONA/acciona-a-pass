import { Component, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { EventEmitter } from 'events';
import { forkJoin, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { IArea, ICountry, ILocation, IRegion, MastersService } from 'src/app/masters.service';
import { FiltersService } from '../employees-filters/filters.service';

@Component({
  selector: 'app-employees-complex-filters',
  templateUrl: './employees-complex-filters.component.html',
  styleUrls: ['./employees-complex-filters.component.scss']
})
export class EmployeesComplexFiltersComponent implements OnInit, OnDestroy {
  @Output() searchEvent = new EventEmitter();

  form: FormGroup;
  divisions: any;
  locations: any;
  countries: ICountry[];
  regions: IRegion[];
  areas: IArea[];

  filteredRegions: IRegion[];
  filteredAreas: IArea[];

  isLoading = true;

  private destroyed$ = new Subject();

  constructor(private masters: MastersService, private service: FiltersService) {}

  trackByLocationId(index: number, item: ILocation) {
    return item.idLocation;
  }

  trackByName(index: number, item: ICountry) {
    return item.name;
  }

  trackById(index: number, item: IRegion | IArea) {
    return item.id;
  }

  doFilter() {
    this.service.set(this.form.value);
  }

  ngOnInit(): void {
    this.form = new FormGroup({
      division: new FormControl([]),
      location: new FormControl([]),
      areas: new FormControl([]),
      regions: new FormControl([]),
      countries: new FormControl([])
    });

    this.form
      .get('countries')
      .valueChanges.pipe(takeUntil(this.destroyed$))
      .subscribe((countries: ICountry[]) => {
        if (countries.length) {
          const countriesNames = countries.map(c => c.name);
          const filtered = this.regions.filter(r => countriesNames.indexOf(r.country) !== -1);
          this.filteredRegions = filtered;
          const previouslyRegions = this.form.get('regions').value;
          this.form.get('regions').patchValue(previouslyRegions.filter(r => countriesNames.indexOf(r.country) !== -1));
        } else {
          this.filteredRegions = this.regions;
        }
      });

    this.form
      .get('regions')
      .valueChanges.pipe(takeUntil(this.destroyed$))
      .subscribe((regions: IRegion[]) => {
        if (regions.length) {
          const regionsId = regions.map(c => c.id);
          const filtered = this.areas.filter(a => regionsId.indexOf(a.idRegion) !== -1);
          this.filteredAreas = filtered;
          const previouslyAreas = this.form.get('areas').value;
          this.form.get('areas').patchValue(previouslyAreas.filter(a => regionsId.indexOf(a.idRegion) !== -1));
        } else {
          this.filteredAreas = this.areas;
        }
      });

    forkJoin([
      this.masters.fetchDivisions(),
      this.masters.fetchCountries(),
      this.masters.fetchRegions(),
      this.masters.fetchAreas(),
      this.masters.fetchLocations()
    ]).subscribe(([divisions, countries, regions, areas, locations]) => {
      this.divisions = divisions;
      this.countries = countries;
      this.regions = regions;
      this.areas = areas;
      this.filteredAreas = areas;
      this.filteredRegions = regions;
      this.locations = locations;
      this.isLoading = false;
    });
  }

  ngOnDestroy() {
    this.destroyed$.next();
  }
}
