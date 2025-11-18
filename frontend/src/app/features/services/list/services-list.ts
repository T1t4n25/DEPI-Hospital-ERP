import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { RippleModule } from 'primeng/ripple';

@Component({
  selector: 'app-services-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, TableModule, ButtonModule, InputTextModule, RippleModule],
  templateUrl: './services-list.html',
  styleUrl: './services-list.css'
})
export class ServicesListComponent {
  services = [
    { id: 1, name: 'General Checkup', cost: '$100', department: 'General Medicine' },
    { id: 2, name: 'Cardiology Consultation', cost: '$200', department: 'Cardiology' },
    { id: 3, name: 'X-Ray', cost: '$150', department: 'Radiology' }
  ];

  searchTerm = '';
}

