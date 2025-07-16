import {Component, OnInit} from '@angular/core';
import {SignalrService} from '../../services/signalr.service';
import {FormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-shopping-list',
  imports: [FormsModule, CommonModule],
  providers: [SignalrService],
  templateUrl: './shopping-list.component.html',
  styleUrl: './shopping-list.component.css'
})
export class ShoppingListComponent implements OnInit{
  newShoppingListItem = '';
  shoppingListIdInput = '';
  shoppingList: string[] = [];
  shoppingListId: string | null = null;
  constructor(private signalrService: SignalrService) { }

  ngOnInit(): void {
    this.connectSignalR();
  }
  private connectSignalR() : void {
    this.signalrService.connect().then(() => {
      this.signalrService.getHubConnection().on("ReceiveShoppingList", (items: string[]) => {
        this.shoppingList = items;
      })
    });
    this.signalrService.getHubConnection().on("ShoppingListCreated", (createdId: string) => {
      this.shoppingListId = createdId;
    })
    this.signalrService.getHubConnection().on("JoinShoppingList", (shoppingListId:string , items:string[]) => {
      this.shoppingListId = shoppingListId;
      this.shoppingList = items;
    })
  }

  addItemToShoppingList() {
    if(this.newShoppingListItem && this.shoppingListId){
      this.signalrService.addItem(this.shoppingListId, this.newShoppingListItem);
      this.newShoppingListItem = '';
    }
  }
  removeItemFromShoppingList(item: string) {
    if (this.shoppingListId){
      this.signalrService.removeItem(this.shoppingListId, item);
    }
  }
  createShoppingList() {
    this.signalrService.createShoppingList();
  }
  joinShoppingList() {
    if (this.shoppingListIdInput){
      this.signalrService.joinShoppingList(this.shoppingListIdInput);
    }
  }
  updateShoppingList(item:string, newItem:string) {
    if (this.shoppingListId){
      this.signalrService.updateShoppingList(item, newItem);
    }
  }
}
