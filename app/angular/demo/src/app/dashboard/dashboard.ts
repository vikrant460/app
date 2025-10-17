import { Component } from '@angular/core';
import { KtdGridLayout, KtdGridModule, ktdTrackById } from '@katoid/angular-grid-layout';
import { NgxTexteditor } from "../ngx-texteditor/ngx-texteditor";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [KtdGridModule, CommonModule, NgxTexteditor],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

  cols: number = 6;
  rowHeight: number = 100;
  layout: KtdGridLayout = [
    { id: '0', x: 0, y: 0, w: 3, h: 3 },
    { id: '1', x: 3, y: 0, w: 3, h: 3 },
    { id: '2', x: 0, y: 3, w: 3, h: 3, minW: 2, minH: 3 },
    { id: '3', x: 3, y: 3, w: 3, h: 3, minW: 2, maxW: 3, minH: 2, maxH: 5 },
  ];

  trackById = ktdTrackById;

  onLayoutUpdated(event: KtdGridLayout) {
    console.log('Layout updated:', event);
  }
}