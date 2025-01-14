import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { StockService } from './stock.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'stockpulse.client';
  symbol: string = '';
  stockData: any;

  constructor(private stockService: StockService) { }

  getStock() {
    this.stockService.getStock(this.symbol).subscribe({
      next: (data) => {
        this.stockData = data;
      },
      error: (err) => {
        console.error('Error fetching stock data', err);
      },
    });
  }
}
