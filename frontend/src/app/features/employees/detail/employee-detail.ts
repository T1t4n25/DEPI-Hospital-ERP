import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-employee-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, CardModule, ButtonModule, TagModule],
  templateUrl: './employee-detail.html',
  styleUrl: './employee-detail.css'
})
export class EmployeeDetailComponent {
  employee = {
    id: 1,
    firstName: 'Dr. John',
    lastName: 'Smith',
    role: 'Doctor',
    department: 'Cardiology',
    contactNumber: '555-1001',
    hireDate: '2020-01-15',
    gender: 'Male'
  };
}

