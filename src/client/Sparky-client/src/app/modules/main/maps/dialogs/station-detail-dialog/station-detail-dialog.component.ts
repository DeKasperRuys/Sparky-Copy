import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ChargingStation } from 'src/app/interfaces/ChargingStations';
import { database } from 'firebase';
import { empty } from 'rxjs';
import { PowerBankSlot } from 'src/app/interfaces/PowerBankSlot';

@Component({
  selector: 'app-station-detail-dialog',
  templateUrl: './station-detail-dialog.component.html',
  styleUrls: ['./station-detail-dialog.component.scss']
})
export class StationDetailDialogComponent implements OnInit {
  fullSlots: PowerBankSlot[];
  constructor(public dialogRef: MatDialogRef<StationDetailDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: ChargingStation) { }

  ngOnInit() {
    this.fullSlots = this.data.slots.filter(x => !x.isEmpty);
  }
  onClose(): void {
    this.dialogRef.close();
  }
}
