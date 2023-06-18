import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Address } from '../../shared/models/Address';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAddress(id: number) {
    return this.http.get<Address>(this.baseUrl + 'address/' + id);
  }

  getAddresses() : Observable<Address[]>{
    return this.http.get<Address[]>(this.baseUrl + 'address/all-addresses');
  }

  addAddress(address: Address) {
    return this.http.post<Address>(this.baseUrl + 'address', address);
  }

  updateAddress(address: Address) {
    return this.http.put<Address>(this.baseUrl + 'address', address);
  }
  
  setCurrentAddress(id: number) {
    return this.http.put<null>(this.baseUrl + 'address/set-current-address', id);
  }

  deleteAddress(id: number) {
    return this.http.delete<null>(this.baseUrl + 'address/' + id);
  }
}
