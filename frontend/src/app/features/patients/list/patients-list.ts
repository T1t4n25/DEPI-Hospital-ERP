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
  selector: 'app-patients-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, TagModule, RippleModule],
  templateUrl: './patients-list.html',
  styleUrl: './patients-list.css'
})
export class PatientsListComponent {
  patients = [
    { id: 1, firstName: 'John', lastName: 'Doe', dateOfBirth: '1985-05-15', gender: 'Male', bloodType: 'O+', contactNumber: '555-0101', address: '123 Main St' },
    { id: 2, firstName: 'Jane', lastName: 'Smith', dateOfBirth: '1990-08-22', gender: 'Female', bloodType: 'A+', contactNumber: '555-0102', address: '456 Oak Ave' },
    { id: 3, firstName: 'Michael', lastName: 'Johnson', dateOfBirth: '1978-12-03', gender: 'Male', bloodType: 'B+', contactNumber: '555-0103', address: '789 Pine Rd' },
    { id: 4, firstName: 'Sarah', lastName: 'Williams', dateOfBirth: '1992-03-18', gender: 'Female', bloodType: 'AB+', contactNumber: '555-0104', address: '321 Elm St' },
    { id: 5, firstName: 'David', lastName: 'Brown', dateOfBirth: '1988-07-25', gender: 'Male', bloodType: 'O-', contactNumber: '555-0105', address: '654 Maple Dr' }
  ];

  searchTerm = '';

  get filteredPatients() {
    if (!this.searchTerm) return this.patients;
    const term = this.searchTerm.toLowerCase();
    return this.patients.filter(p => 
      p.firstName.toLowerCase().includes(term) ||
      p.lastName.toLowerCase().includes(term) ||
      p.contactNumber.includes(term)
    );
  }
}

