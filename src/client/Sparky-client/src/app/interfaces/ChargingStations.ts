import { PowerBankSlot } from "./PowerBankSlot";
export interface ChargingStation {
  chargingStationId: number;
  name: string;
  lat: number;
  lon: number;
  numOfSlots: number;
  isRequested: boolean;
  slots: PowerBankSlot[];
  mlat: any;
  mlon: any;
}
