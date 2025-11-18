import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-employees-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './employees-list.html',
  styleUrl: './employees-list.css'
})
export class EmployeesListComponent {
  employees = [
    { id: 1, firstName: 'Dr. John', lastName: 'Smith', role: 'Doctor', department: 'Cardiology', contactNumber: '555-1001', hireDate: '2020-01-15' },
    { id: 2, firstName: 'Dr. Jane', lastName: 'Johnson', role: 'Doctor', department: 'Pediatrics', contactNumber: '555-1002', hireDate: '2019-03-20' },
    { id: 3, firstName: 'Mary', lastName: 'Williams', role: 'Nurse', department: 'Emergency', contactNumber: '555-1003', hireDate: '2021-06-10' }
  ];

  searchTerm = '';

  get filteredEmployees() {
    if (!this.searchTerm) return this.employees;
    const term = this.searchTerm.toLowerCase();
    return this.employees.filter(e => 
      e.firstName.toLowerCase().includes(term) ||
      e.lastName.toLowerCase().includes(term) ||
      e.role.toLowerCase().includes(term) ||
      e.department.toLowerCase().includes(term)
    );
  }
}

