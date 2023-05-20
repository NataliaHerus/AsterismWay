import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { EventService } from 'src/app/demo/services/app services/event.service';
import {Event} from 'src/app/demo/models/event';
import { Frequency } from 'src/app/demo/models/frequency';
import { Category } from 'src/app/demo/models/category';

@Component({
  selector: 'app-events-manager',
  templateUrl: './events-manager.component.html',
  styleUrls: ['./events-manager.component.scss'],
  providers: [MessageService]
})

export class EventsManagerComponent implements OnInit {

  events: Event[] = [];

  eventDialog: boolean = false;
  deleteEventDialog: boolean = false;
  deleteEventsDialog: boolean = false;
  createEventDialog:  boolean = false;

  event: Event = {};
  selectedEvents: Event[] = [];

  submitted: boolean = false;
  cols: any[] = [];

  categories: any[] = [];
  frequencies: any[]=[];
  frequency: string='';
  category: string='';

  uploadedFile: File | undefined;



  rowsPerPageOptions = [5, 10, 20];

  constructor(private eventService: EventService, private messageService: MessageService) { }

  ngOnInit() {
    this.eventService.getAll().subscribe(events => {
        this.events = events;
    });
    this.categories = [
      { label: 'Планети', value: 'планети' },
      { label: 'Зорі', value: 'зорі' },
      { label: 'Місяць', value: 'місяць' },
      { label: 'Сонце', value: 'сонце' },
      { label: 'Метеорити', value: 'метеорити' },
  ];

  this.frequencies = [
    { label: 'Часто', value: 'часто' },
    { label: 'Відносно часто', value: 'відносно часто' },
    { label: 'Рідко', value: 'рідко' },
    { label: 'Дуже рідко', value: 'дуже рідко' },
    { label: 'Майже ніколи', value: 'майже ніколи' },
    { label: 'Раз у рік', value: 'раз у рік' },
];
  }

  onFileSelected(event: any) {
    this.uploadedFile = event.target.files[0];
  }

  openNew() {
    this.createEventDialog = true
    this.event = {};
    this.submitted = false;
    this.eventDialog = true;
}

deleteSelectedProducts() {
    this.deleteEventsDialog = true;
}

editProduct(event: Event) {
    this.createEventDialog = false
    this.event = { ...event };
    if (this.frequency != '')
    {
     this.event.frequency!.name = this.frequency;
    }
    if (this.category != '')
    {
        this.event.category!.name = this.category;
    }
    this.eventDialog = true;
}

deleteProduct(event: Event) {
    this.deleteEventDialog = true;
    this.event = { ...event };
}

confirmDeleteSelected() {
    this.deleteEventsDialog = false;
    this.events = this.events.filter(val => !this.selectedEvents.includes(val));
    for (let i = 0; i < this.selectedEvents.length; i++) {
    this.eventService.deleteById(this.selectedEvents[i].id!).subscribe(data => {
        this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Подію видалено'})});
    }
    this.selectedEvents = [];
}

confirmDelete() {
    this.deleteEventDialog = false;
    this.events = this.events.filter(val => val.id !== this.event.id);
    this.eventService.deleteById(this.event.id!).subscribe(data => {
    this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Подію видалено' })});
    this.event = {};
}

hideDialog() {
    this.eventDialog = false;
    this.submitted = false;
    this.createEventDialog = false;
}


  downloadFile() {
    console.log(this.uploadedFile)
    if (this.uploadedFile) {
        console.log(this.uploadedFile.name)
      /*const url = window.URL.createObjectURL(this.uploadedFile);
      const link = document.createElement('a');
      link.href = url;
      link.download = this.uploadedFile.name;
      link.click();*/
    }
  }

saveProduct(event: Event) {
   this.submitted = true;

    if (this.event.name?.trim()) {
        if (this.event.id) {
            this.eventService.update(this.event).subscribe(data => {
                this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Подію оновлено', life: 3000 });
            });
            this.events[this.findIndexById(this.event.id!)] = this.event;
        } 
        else {
            console.log(this.frequency)
            console.log(this.category)
            console.log(event)
            let fr: Frequency = {name: this.frequency}
            let ct: Category = {name: this.category}
            this.event.name = event.name;
            this.event.description = event.description;
            this.event.startDate = event.startDate;
            this.event.endDate = event.endDate;
            this.event.frequency=fr;
            this.event.category = ct;
            this.eventService.create(this.event).subscribe(data => {
                this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Подію створено', life: 3000 });
            });

            this.createEventDialog = false;
            this.events.push(this.event);
            if (this.uploadedFile) {
                const chunk = this.uploadedFile.slice(0, this.uploadedFile.size);
                console.log(chunk)
                this.eventService.uploadFile(chunk).subscribe( {});
            }
        }
        this.events = [...this.events];
        this.eventDialog = false;
        this.event = {};
        this.category='';
        this.frequency='';
    }
}

onFrequencyChange(event: Event) {
    console.log(event.frequency!.name); // Print the selected value (optional)

  // Find the selected frequency object based on the label
  this.frequency = this.frequencies.find(frequency => frequency.label === event.frequency!.name);
  
  }

  onCategoryChange(event: Event) {
    console.log(event.category!.name);
  this.category = this.categories.find(category => category.label === event.category!.name);
  
  }

findIndexById(id: number): number {
    let index = -1;
    for (let i = 0; i < this.events.length; i++) {
        if (this.events[i].id === id) {
            index = i;
            break;
        }
    }

    return index;
}


onGlobalFilter(table: Table, event: any) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
}


}
