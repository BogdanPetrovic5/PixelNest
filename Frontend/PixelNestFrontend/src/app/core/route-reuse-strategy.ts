import { ActivatedRouteSnapshot, DetachedRouteHandle, Router, RouteReuseStrategy } from "@angular/router";
import { AuthenticationService } from "./services/authentication/authentication.service";
import { inject, Inject, Injectable, Injector } from "@angular/core";
import { each } from "hammerjs";
import { AuthStateService } from "./services/states/auth-state.service";
@Injectable({
  providedIn: 'root'
})
export class CustomRouteReuseStrategy implements RouteReuseStrategy {
    private storedRoutes = new Map<string, DetachedRouteHandle>();
    private scrollPositions = new Map<string, number>();
    handlers: {[key: string]: DetachedRouteHandle} = {};
    copyRoutes:any;
    private _router!: Router;
    constructor(private _authState:AuthStateService,  private _injector: Injector){
      setTimeout(() => {
        this._router = this._injector.get(Router);
      });
      _authState.isLoggedIn$.subscribe({
        next:response=>{
          if(response == false) this.destroyComponents()
        }
      })
    }
    private getFullRouteUrl(route: ActivatedRouteSnapshot): string {
         let path = route.routeConfig?.path ?? '';
  let parent = route.parent;

  while (parent) {
    const parentPath = parent.routeConfig?.path ?? '';
   path = `${parentPath}/${path}`.replace(/\/+/g, '/');
    parent = parent.parent;
  }

  return path;
      }
    
      shouldDetach(route: ActivatedRouteSnapshot): boolean {
        const fullPath = this.getFullRouteUrl(route);
 
        return fullPath === '/dashboard/' || fullPath === '/dashboard/feed';
      }
    
      store(route: ActivatedRouteSnapshot, handle: DetachedRouteHandle | null): void {
        const fullPath = this.getFullRouteUrl(route);
        if (handle && (fullPath === '/dashboard/' || fullPath === '/dashboard/feed')) {
          this.storedRoutes.set(fullPath, handle);
    
        }
      
       
      }
    
      shouldAttach(route: ActivatedRouteSnapshot): boolean {
        const fullPath = this.getFullRouteUrl(route);
        const canAttach = this.storedRoutes.has(fullPath);

        return canAttach;
      }
    
      retrieve(route: ActivatedRouteSnapshot): DetachedRouteHandle | null {
        const fullPath = this.getFullRouteUrl(route);
        const handle = this.storedRoutes.get(fullPath) || null;

        return handle;
      }
    
      shouldReuseRoute(future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean {
        const blockedRoutes = ['/authentication/login', '/authentication/register', '/unauthorized'];
        const currUrl = '/' + curr.url.map(segment => segment.path).join('/');
        
   

        if (blockedRoutes.includes(currUrl)) {
          
          return false; 
        }

        return future.routeConfig === curr.routeConfig;
      }
    restoreScrollPosition(route: ActivatedRouteSnapshot): void {
        const fullPath = this.getFullRouteUrl(route)
        const scrollPosition = this.scrollPositions.get(fullPath);
        if (scrollPosition !== undefined) {
        
          window.scrollTo(0, scrollPosition); 
        }
    }
    destroyComponents() {



    this.storedRoutes.forEach((handle, key) => {
     
        
        if (handle && (handle as any).componentRef) {
          (handle as any).componentRef.destroy();
        
        }
        this.storedRoutes.delete(key);
   
    });
 
   

    }
    
}