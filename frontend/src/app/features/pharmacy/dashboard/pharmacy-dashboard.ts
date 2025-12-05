import { Component, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartModule } from 'primeng/chart';
import { CardModule } from 'primeng/card';
import { MedicationsService } from '../../medications/services/medications.service';
import { InventoryService } from '../../inventory/services/inventory.service';
import { forkJoin } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-pharmacy-dashboard',
  standalone: true,
  imports: [CommonModule, ChartModule, CardModule],
  templateUrl: './pharmacy-dashboard.html',
  styleUrl: './pharmacy-dashboard.css'
})
export class PharmacyDashboardComponent {
  stats = [
    { label: 'Total Medications', value: '...', icon: 'pi pi-box', color: 'bg-blue-500' },
    { label: 'Low Stock Items', value: '...', icon: 'pi pi-exclamation-triangle', color: 'bg-yellow-500' },
    { label: 'Expiring Soon', value: '...', icon: 'pi pi-calendar-times', color: 'bg-red-500' },
    { label: 'Total Value', value: '...', icon: 'pi pi-dollar', color: 'bg-green-500' }
  ];

  medicationChartData: any;
  chartOptions: any;

  constructor(
    private medicationsService: MedicationsService,
    private inventoryService: InventoryService,
    private destroyRef: DestroyRef
  ) {
    this.initChart();
    this.loadDashboardData();
  }

  private initChart() {
    this.medicationChartData = {
      labels: ['Cardiology', 'Antibiotics', 'Pain Relief', 'Vitamins', 'Others'],
      datasets: [{
        data: [30, 25, 20, 15, 10], // Placeholder for now as we don't have category data
        backgroundColor: ['#0ea5e9', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6']
      }]
    };

    this.chartOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.6
    };
  }

  private loadDashboardData() {
    // ForkJoin to get both datasets
    forkJoin({
      medications: this.medicationsService.getAll({ pageSize: 10000 }),
      inventory: this.inventoryService.getAll({ pageSize: 10000 })
    }).pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe({
      next: ({ medications, inventory }) => {
        this.calculateStats(medications.data, inventory.data);
      },
      error: (err) => console.error('Failed to load dashboard data', err)
    });
  }

  private calculateStats(medications: any[], inventory: any[]) {
    // 1. Total Medications
    const totalMedications = medications.length;

    // 2. Low Stock Items (Quantity < 10)
    const lowStockCount = inventory.filter(item => item.quantity < 10).length;

    // 3. Expiring Soon (Within 30 days)
    const today = new Date();
    const thirtyDaysFromNow = new Date();
    thirtyDaysFromNow.setDate(today.getDate() + 30);

    const expiringSoonCount = inventory.filter(item => {
      const expiryDate = new Date(item.expiryDate);
      return expiryDate >= today && expiryDate <= thirtyDaysFromNow;
    }).length;

    // 4. Total Value (Quantity * Cost)
    let totalValue = 0;
    inventory.forEach(item => {
      const med = medications.find(m => m.medicationID === item.medicationID);
      if (med) {
        totalValue += item.quantity * med.cost;
      }
    });

    // Update stats array
    this.stats = [
      { label: 'Total Medications', value: totalMedications.toString(), icon: 'pi pi-box', color: 'bg-blue-500' },
      { label: 'Low Stock Items', value: lowStockCount.toString(), icon: 'pi pi-exclamation-triangle', color: 'bg-yellow-500' },
      { label: 'Expiring Soon', value: expiringSoonCount.toString(), icon: 'pi pi-calendar-times', color: 'bg-red-500' },
      { label: 'Total Value', value: `$${totalValue.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`, icon: 'pi pi-dollar', color: 'bg-green-500' }
    ];
  }
}

