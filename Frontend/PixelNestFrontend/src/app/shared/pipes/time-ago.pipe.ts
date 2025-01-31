import { Pipe, PipeTransform } from '@angular/core';
import { addMinutes, addSeconds, formatDistance, formatDistanceToNow } from 'date-fns';
@Pipe({
  name: 'timeAgo'
})
export class TimeAgoPipe implements PipeTransform {

  transform(value: Date | string): unknown {
    if (!value) return '';

    const date = new Date(value);
   
    const now = new Date();
    const utcDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
   
    const seconds = Math.floor((+now - +utcDate) / 1000);
    const minutes = Math.floor(seconds / 60);
    const hours = Math.floor(minutes / 60);
    const days = Math.floor(hours / 24);
    const months = Math.floor(days / 30);
    const years = Math.floor(days / 365);

    if (seconds < 60) {
      return `${seconds} second${seconds !== 1 ? 's' : ''} ago`;
    }
    if (minutes < 60) {
      return `${minutes} minute${minutes !== 1 ? 's' : ''} ago`;
    }
    if(hours < 24){
      return `${hours} hour${hours !== 1 ? 's' : ''} ago`
    }
    if (days < 30) {
      return `${days} day${days !== 1 ? 's' : ''} ago`;
    }

   
    if (months < 12) {
      return `${months} month${months !== 1 ? 's' : ''} ago`;
    }
    return formatDistance(value, Date.now(), {addSuffix: true});
  
  }

}
