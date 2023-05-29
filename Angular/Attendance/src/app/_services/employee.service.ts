import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Employee } from '../Models/employee';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  employees: Employee[] = [];
  totalTimeMap = new Map<string, number>();
  baseUrl: string =
    'https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Employee[]>(this.baseUrl);
  }

  getTotalTimeWorked(): Observable<Map<string, number>> {
    return this.getAll().pipe(
      map((employees) => {
        this.employees = employees;
        this.calculateTotalTimeWorked();
        return this.totalTimeMap;
      })
    );
  }

  private calculateTotalTimeWorked() {
    this.totalTimeMap.clear();
    this.employees.forEach((employee) => {
      const startTime = new Date(employee.StarTimeUtc);
      const endTime = new Date(employee.EndTimeUtc);
      const totalTime =
        (endTime.getTime() - startTime.getTime()) / 1000 / 60 / 60;
      if (this.totalTimeMap.has(employee.EmployeeName)) {
        this.totalTimeMap.set(
          employee.EmployeeName,
          totalTime + (this.totalTimeMap.get(employee.EmployeeName) || 0)
        );
      } else {
        this.totalTimeMap.set(employee.EmployeeName, totalTime);
      }
    });
  }
}
