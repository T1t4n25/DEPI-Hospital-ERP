import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { TagModule } from 'primeng/tag';

@Component({
  selector: 'app-hr-employees',
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, InputTextModule, TagModule],
  templateUrl: './hr-employees.html',
  styleUrl: './hr-employees.css'
})
export class HrEmployeesComponent {
  employees = [
    { id: 1, name: 'Dr. John Smith', department: 'Cardiology', role: 'Doctor', status: 'Active' },
    { id: 2, name: 'Mary Williams', department: 'Emergency', role: 'Nurse', status: 'Active' }
  ];

  searchTerm = '';
}

