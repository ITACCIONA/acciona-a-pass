import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MastersService } from 'src/app/masters.service';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss']
})
export class EmployeeComponent implements OnInit {
  id: string;
  role: string;

  constructor(private route: ActivatedRoute, private masters: MastersService) {}

  ngOnInit(): void {
    this.id = this.route.snapshot.params.id;
    this.role = this.masters.getRole();
  }
}
