import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-departments-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, RippleModule],
  templateUrl: './departments-list.html',
  styleUrl: './departments-list.css'
})
export class DepartmentsListComponent {
  departments = [
    { id: 1, name: 'Cardiology', manager: 'Dr. John Smith', employeeCount: 15 },
    { id: 2, name: 'Pediatrics', manager: 'Dr. Jane Johnson', employeeCount: 12 },
    { id: 3, name: 'Emergency', manager: 'Dr. Michael Williams', employeeCount: 20 }
  ];

  searchTerm = '';

  get filteredDepartments() {
    if (!this.searchTerm) return this.departments;
    const term = this.searchTerm.toLowerCase();
    return this.departments.filter(d => 
      d.name.toLowerCase().includes(term) ||
      d.manager.toLowerCase().includes(term)
    );
  }
}

