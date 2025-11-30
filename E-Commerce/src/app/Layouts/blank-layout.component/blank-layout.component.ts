import { Component, inject, InjectionToken, PLATFORM_ID, signal, WritableSignal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { trigger, state, style, animate, transition } from '@angular/animations';
import { CommonModule, isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-blank-layout.component',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './blank-layout.component.html',
  styleUrl: './blank-layout.component.scss',
  animations: [
    trigger("toggleSideNavAnimation", [
      state("open", style({ left: "0" })),
      state("close", style({ left: "-16rem" })),
      transition("open <=> close", animate("1s ease-in-out"))
    ]),
    trigger("toggleNavAnimation", [
      state("open", style({ marginLeft: "20rem"})),
      state("close", style({ marginLeft: "4rem" })),
      transition("open <=> close", animate("1s ease-in-out"))
    ]),
  ]
})
export class BlankLayoutComponent {
  currentWindowWidth : WritableSignal<number> = signal(0);
  private PLATFORM_ID = inject(PLATFORM_ID);
  private readonly _router = inject(Router);
  ngOnInit(){
    if(isPlatformBrowser(this.PLATFORM_ID)){
      document.documentElement.classList.toggle("dark", localStorage.getItem("mode") == "dark");
      this.currentWindowWidth.set(window.innerWidth);
      window.addEventListener('resize', this.updateWindowWidth);
    }
  }

  updateWindowWidth = () => {
    this.currentWindowWidth.set(window.innerWidth);
  };

  SideNavState : WritableSignal<string> = signal("close");
  toggleSideNav(){
    this.SideNavState.update((state) => state === "close" ? "open" : "close");
  }

  ngOnDestroy() {
    if(isPlatformBrowser(this.PLATFORM_ID)){
      window.removeEventListener('resize', this.updateWindowWidth);
    }
  }

  toggleDarkMode(){
    if(localStorage.getItem("mode") == "dark"){
      localStorage.setItem("mode", "light");
      document.documentElement.classList.toggle("dark", false);
    }
    else{
      localStorage.setItem("mode", "dark");
      document.documentElement.classList.toggle("dark", true);
    }
  }
  
  LogOut(){
    localStorage.removeItem("token");
    this._router.navigate(['/Login']);
  }
}
