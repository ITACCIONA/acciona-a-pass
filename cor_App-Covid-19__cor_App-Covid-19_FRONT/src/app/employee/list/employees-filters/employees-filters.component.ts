import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MastersService } from '../../../masters.service';
import { SidenavService } from '../../../ui/sidenav/sidenav.service';
import { EmployeesComplexFiltersComponent } from '../employees-complex-filters/employees-complex-filters.component';

@Component({
  selector: 'app-employees-filters',
  templateUrl: './employees-filters.component.html',
  styleUrls: ['./employees-filters.component.scss']
})
export class EmployeesFiltersComponent implements OnInit {
  form: FormGroup;
  searchKey: string;

  @Input() filtersBtn = true;
  @Input() exportBtn = true;
  @Input() additional = false;

  @Output() searchEvent = new EventEmitter<string>();

  @Output() export = new EventEmitter<void>();

  constructor(private fb: FormBuilder, private sidenav: SidenavService, private master: MastersService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      search: ''
    });
  }

  search() {
    this.searchKey = this.form.value.search;
    this.searchEvent.emit(this.searchKey);
  }

  openFilters() {
    if (!this.sidenav.content.value) {
      this.sidenav.setContent(EmployeesComplexFiltersComponent);
    }
    this.sidenav.toggle();
  }

  exportLaunch() {
    this.export.emit();
  }
}
