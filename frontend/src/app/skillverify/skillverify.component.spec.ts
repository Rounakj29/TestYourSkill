import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillverifyComponent } from './skillverify.component';

describe('SkillverifyComponent', () => {
  let component: SkillverifyComponent;
  let fixture: ComponentFixture<SkillverifyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SkillverifyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SkillverifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
