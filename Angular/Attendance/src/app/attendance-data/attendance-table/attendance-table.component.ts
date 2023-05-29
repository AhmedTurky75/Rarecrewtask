import { Component } from '@angular/core';
import { Employee } from 'src/app/Models/employee';
import { EmployeeService } from 'src/app/_services/employee.service';

@Component({
  selector: 'app-attendance-table',
  templateUrl: './attendance-table.component.html',
  styleUrls: ['./attendance-table.component.css'],
})
export class AttendanceTableComponent {
  totalTimeMap = new Map<string, number>();

  constructor(public employeeService: EmployeeService) {
    employeeService.getTotalTimeWorked().subscribe((totalTimeMap) => {
      this.totalTimeMap = totalTimeMap;
    });
  }
}
