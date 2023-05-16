import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Dialog } from 'primeng/dialog';
import { DialogService } from 'primeng/dynamicdialog';
import { SelectedEvent } from 'src/app/demo/models/selectedEvent';
import { User } from 'src/app/demo/models/user';
import { SelectedEventsService } from 'src/app/demo/services/app services/selectedEvents.service';
import { AuthService } from 'src/app/demo/services/identity services/auth.service';
import { EventDetailsComponent } from '../../event/event-details/event-details.component';
import { SelectItem } from 'primeng/api';
import { DataView } from 'primeng/dataview';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.scss'],
  providers: [DialogService]
})
export class UserInfoComponent implements OnInit {

  user?: User;
  events: SelectedEvent []=[];
  eventDialog!: Dialog;
  sortOptions: SelectItem[] = [];
  sortOrder: number = 0;
  sortField: string = '';
  @ViewChild('filter') filter!: ElementRef;
  
  constructor(private _authService: AuthService,
    private _selectedEventsService: SelectedEventsService, private dialogService: DialogService) { }

  ngOnInit(): void {
     this._authService.getCurrentUser().subscribe(user => {
      this.user = user;
  });

  this._selectedEventsService.getAllByUser().subscribe(events => {
    this.events = events;
});

  this.sortOptions = [
    { label: 'Незабаром', value: 'startDate' },
    { label: 'Пізніші', value: '!startDate' }
  ]
  }

  onSortChange(event: any) {
    const value = event.value;

    if (value.indexOf('!') === 0) {
        this.sortOrder = -1;
        this.sortField = value.substring(1, value.length);
    } else {
        this.sortOrder = 1;
        this.sortField = value;
    }
}

onFilter(dv: DataView, event: any) {
    dv.filter((event.target as HTMLInputElement).value);
}

showEventDetails(id: number) { 
  this.dialogService.open(EventDetailsComponent, {
    header: 'Деталі про подію',
    width: '70%',
    data: {id: id },
  });

}

}
