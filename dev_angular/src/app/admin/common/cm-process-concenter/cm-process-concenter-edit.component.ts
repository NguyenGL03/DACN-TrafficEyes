import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, Injector, input, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { DefaultComponentBase } from '@app/utilities/default-component-base';
import { EditPageState } from '@app/utilities/enum/edit-page-state';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AllCodeServiceProxy, CM_ALLCODE_ENTITY, CM_PROCESS_ENTITY, CM_PROCESS_LIST_ENTITY, CM_PROCESS_STATUS_ENTITY, ProcessServiceProxy, RoleListDto, RoleServiceProxy, UltilityServiceProxy } from "@shared/service-proxies/service-proxies";
import { finalize } from 'rxjs';
import { BranchedStep, ComponentType, Definition, Designer, RootEditorContext, Step, StepEditorContext, StepsConfiguration, ToolboxConfiguration, Uid, ValidatorConfiguration } from 'sequential-workflow-designer';

@Component({
  selector: 'cm-process-concenter',
  templateUrl: './cm-process-concenter-edit.component.html',
  animations: [appModuleAnimation()],
  encapsulation: ViewEncapsulation.None
})
export class CmProcessConcenterEditComponent extends DefaultComponentBase implements IUiAction<CM_PROCESS_LIST_ENTITY>, OnInit, AfterViewInit {

  @ViewChild('editForm') editForm: ElementRef;
  filterInput: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY();
  EditPageState = EditPageState;
  editPageState: EditPageState;
  isShowError = false;
  isApproveFunct: boolean;

  inputModel: CM_PROCESS_LIST_ENTITY = new CM_PROCESS_LIST_ENTITY();
  formData: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY();

  // Cac list danh cho combobox
  processList: CM_PROCESS_LIST_ENTITY[] = [];
  subProcessList: CM_PROCESS_ENTITY[] = [];
  stepList: Step[] = [];
  saveState: boolean = false;
  statusList: { name: string }[] = [];
  roleList: RoleListDto[] = [];
  orderList: { idx: number }[] = [];
  fromList: { idx: string }[] = [];
  conditionStatusList: { name: string }[] = [];
  nameActionList: CM_ALLCODE_ENTITY[] = [];
  fromStatusList: CM_ALLCODE_ENTITY[] = [];
  actionList: CM_ALLCODE_ENTITY[] = [];
  rangeProcessList: CM_ALLCODE_ENTITY[] = [];
  processStatusList: CM_PROCESS_STATUS_ENTITY[] = [];
  prevProcessStatusList: CM_PROCESS_STATUS_ENTITY[] = [];

  designer?: Designer;
  definition: Definition = this.createDefinition();
  copyDefinition: Definition = this.createDefinition();
  definitionJSON?: string;
  selectedStepId: string | null = null;
  isReadonly = false;
  isToolboxCollapsed = false;
  isEditorCollapsed = false;
  isLoading = true;
  isValid?: boolean;
  nowStatus: string;
  updateFirstPrev: boolean = true;

  constructor(
    injector: Injector,
    private processService: ProcessServiceProxy,
    private _roleService: RoleServiceProxy,
    private ultilityService: UltilityServiceProxy,
    private allCodeServiceProxy: AllCodeServiceProxy,
    private changeDetector: ChangeDetectorRef,
  ) {
    super(injector);
    this.editPageState = this.getRouteData('editPageState');
    this.inputModel.procesS_KEY = this.getRouteParam('id');
    this.initFilter();
    this.initCombobox();
    this.initIsApproveFunct();
  }

  ngOnInit(): void {
    switch (this.editPageState) {
      case EditPageState.add:
        this.inputModel.grouP_STATUS_DONE = 'D';
        this.inputModel.grouP_STATUS_DONE_NAME = 'Hoàn tất';
        break;

      default:
        this.showLoading();
        this.processService.cM_PROCESS_LIST_ById(this.inputModel.procesS_KEY)
          .pipe(finalize(() => this.hideLoading()))
          .subscribe(response => {
            this.inputModel = response;
            this.initStatusList();
            this.loadWorkflow();
            this.updateDefinitionJSON();
          })
        break;
    }
  }

  ngAfterViewInit(): void {
    this.changeDetector.detectChanges();
  }

  // #region Configuration

  validatorConfiguration: ValidatorConfiguration = {
    // step: (step: Step) => !!step.name && Number(step.properties['velocity']) >= 0,
    // root: (definition: Definition) => Number(definition.properties['velocity']) >= 0
  };

  stepsConfiguration: StepsConfiguration = {
    iconUrlProvider: (componentType: ComponentType, type: string) => {
      return this.resolveImgURL(type)
    }
  };

  resolveImgURL(type) {
    if (type == 'if') {
      return 'https://nocode-js.github.io/sequential-workflow-designer/examples/assets/icon-if.svg'
    }

    if (type == 'parallel') {
      return 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT_SYr6SJGjp21aJTiQWaSzc_5ET6GXtXueWQ&s'
    }

    if (type == 'f') {
      return 'https://img.icons8.com/?size=100&id=108294&format=png&color=000000'
    }

    if (type == 'manager') {
      return 'https://img.icons8.com/?size=100&id=psevkzUhHRTs&format=png&color=000000'
    }

    if (type == 'group') {
      return 'https://img.icons8.com/?size=100&id=108314&format=png&color=000000'
    }

    return "https://img.icons8.com/?size=100&id=108294&format=png&color=000000"
  }

  // #endregion Configuration

  // #region Cấu hình toolbox
  toolboxConfiguration: ToolboxConfiguration = {
    groups: [
      {
        name: 'Components',
        steps: [this.createParallel()]
      },
      {
        name: 'Processes',
        steps: [this.createProcess(), this.createApproveGroup()]
      }
    ]
  };

  onDesignerReady(designer: Designer) {
    this.designer = designer;
    this.updateIsValid();
  }
  // #endregion - Cấu hình toolbox

  // #region Event tạo component default

  // #TODO - Tạo default Definition
  createDefinition(): Definition {
    return { properties: {}, sequence: [] };
  }

  // #TODO - Event tạo quy trình
  createProcess(): Step {
    const proc: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY()
    proc.procesS_KEY = this.getRouteParam('id');

    return {
      id: Uid.next(),
      componentType: 'task',
      name: 'Process',
      type: 'process',
      properties: {
        ["process"]: proc,
        ["parCond"]: [] as { parent: string[]; condition: string[] }[]
      }
    };
  }

  // #TODO - Event tạo quy trình
  createApproveGroup(): Step {
    const proc: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY()
    proc.procesS_KEY = this.getRouteParam('id');

    return {
      id: Uid.next(),
      componentType: 'task',
      name: 'Group',
      type: 'group',
      properties: {
        ["process"]: proc,
        ["parCond"]: [] as { parent: string[]; condition: string[] }[]
      }
    };
  }

  // #TODO - Event tạo cấu trúc rẽ nhánh
  createParallel(): BranchedStep {
    const firsTimeAddConditionText = ''
    return {
      id: Uid.next(),
      componentType: 'switch',
      name: 'Parallel',
      type: 'parallel',
      properties: {
        conditionsObj: { ['Branch 1']: firsTimeAddConditionText, ['Branch 2']: firsTimeAddConditionText }
      },
      branches: {
        ['Branch 1']: [],
        ['Branch 2']: []
      }
    };
  }
  // #endregion #TODO - Event tạo component default

  // #region Khởi tạo dữ liệu
  initStatusList() {

    const filterPrevProcess = new CM_PROCESS_STATUS_ENTITY();
    filterPrevProcess.procesS_KEY = this.inputModel.preV_PROCESS_KEY;
    this.processService.cM_PROCESS_GetStatusByProcess(filterPrevProcess)
      .subscribe(result => {
        this.prevProcessStatusList = result;

      })

    const filterProcess = new CM_PROCESS_STATUS_ENTITY();
    filterProcess.procesS_KEY = this.inputModel.procesS_KEY;
    this.processService.cM_PROCESS_GetStatusByProcess(filterProcess)
      .subscribe(result => {
        this.processStatusList = result;
      })
  }
  initCombobox() {
    // this.generateStatusList();
    // this.conditionStatusList = this.statusList;

    this.processService.cM_PROCESS_LIST_Search(this.getFillterForCombobox())
      .subscribe((response) => {
        this.processList = response.items;
      })


    if (!this.inputModel.procesS_KEY) {
      // setTimeout(() => {
      this.generateDefaultOrderList();
      this.fromList = this.orderList.map((item) => { return { idx: `${item.idx}` } });
      this.isLoading = false;
      // }, 2000)
    } else {
      this.processService.cM_PROCESS_ByProcess(this.inputModel.procesS_KEY).subscribe(response => {
        this.generateOrderList(response.items.length, 5);
        this.fromList = this.orderList.map((item) => { return { idx: `${item.idx}` } });
        this.isLoading = false;
      });
    }

    this._roleService.getRoles(undefined, undefined).subscribe((result) => {
      this.roleList = result.items.map(item => ({ ...item, name: item.displayName } as RoleListDto));
    })

    this.allCodeServiceProxy.cM_ALLCODE_GetByCDNAME('P_NACTION', 'CM_PROCESS').subscribe(result => {
      this.nameActionList = result;
    });

    this.allCodeServiceProxy.cM_ALLCODE_GetByCDNAME('P_FSTATUS', 'CM_PROCESS').subscribe(result => {
      this.fromStatusList = result;
    });

    this.allCodeServiceProxy.cM_ALLCODE_GetByCDNAME('P_ACTION', 'CM_PROCESS').subscribe(result => {
      this.actionList = result;
    });

    this.allCodeServiceProxy.cM_ALLCODE_GetByCDNAME('P_RANGE_PROC', 'CM_PROCESS').subscribe(result => {
      this.rangeProcessList = result;
    });
  }

  // #endregion Khởi tạo dữ liệu

  // #region Load quy trình 

  // Mỗi khi quy trình có sự thay đổi, load quy trình lại từ đầu
  // Các event thay đổi properties của step sẽ kích hoạt event
  onDefinitionChanged(definition: Definition) {
    this.nowStatus = this.inputModel?.preV_GROUP_STATUS_DONE || '';
    this.updateFirstPrev = true;
    // track all processes
    this.subProcessList = [];
    this.stepList = [];
    this.definition = definition;

    // Một process có thể phụ thuộc vào nhiều quy trình và điều kiện tương ứng
    let parCond: { parent: string[]; condition: string[] }[] = [{ parent: [], condition: [] }];
    for (let i = 0; i < this.definition.sequence.length; i++) {
      parCond = this.dependtrack(this.definition.sequence[i], parCond);
    }

    this.updateIsValid();
    this.updateDefinitionJSON();
  }

  // Hàm đệ quy load quy trình
  dependtrack(st: Step, parCond: { parent: string[]; condition: string[] }[]): { parent: string[]; condition: string[] }[] {
    if (st.type == 'process' || st.type == 'group') {
      if (this.updateFirstPrev && parCond.length > 0 && parCond[0].parent) {
        parCond[0].parent.push(this.inputModel.preV_GROUP_STATUS_DONE);
      }
      st.properties.parCond = parCond;

      (st.properties.process as CM_PROCESS_ENTITY).status = this.nextStatus();
      this.stepList.push(st);
      this.updateFirstPrev = false;
      return [{ parent: [(st.properties.process as CM_PROCESS_ENTITY).status], condition: [] }]
    } else {
      let list_parCond: { parent: string[]; condition: string[] }[] = []
      for (let [branch, sequence] of Object.entries((st as BranchedStep).branches)) {
        let temp_parCond = JSON.parse(JSON.stringify(parCond))
        if ((st as BranchedStep).properties.conditionsObj[branch].length > 0) {
          parCond.forEach((pCond) => pCond.condition.push((st as BranchedStep).properties.conditionsObj[branch]))
        }

        for (let i = 0; i < sequence.length; i++) {
          let par = this.dependtrack(sequence[i], parCond)
          parCond = par
        }
        for (let p of parCond) {
          if (!JSON.stringify(list_parCond).includes(JSON.stringify(p))) {
            list_parCond.push(p)
          }
        }
        parCond = temp_parCond
      }
      return list_parCond
    }
  }

  //TODO - Event tự khởi tạo trạng thái tiếp theo
  nextStatus(): string {
    this.nowStatus = (!this.nowStatus) ? 'A' : String.fromCharCode(this.nowStatus.charCodeAt(0) + 1);
    // R - Là status từ chối của quy trình
    if (
      this.nowStatus == 'R'
      || this.nowStatus == this.inputModel.preV_GROUP_STATUS_DONE
      || this.nowStatus == this.inputModel.grouP_STATUS_DONE
    )
      return this.nextStatus();
    return this.nowStatus
  }

  // #endregion Load quy trình 

  // #region Event select quy trình 

  // Hàm đệ quy truy vết quy trình
  onTrack(stepId: string | null, st: Step): any {
    if (st['type'] == 'process' || st['type'] == 'group') {
      return (st['id'] == stepId) ? st.properties.process : null
    }

    for (let [branch, sequence] of Object.entries((st as BranchedStep).branches)) {
      for (let item of sequence) {
        if (this.onTrack(stepId, item)) {
          return this.onTrack(stepId, item)
        }
      }
    }
    return null
  }

  // Truy tìm process trong quy trình mỗi khi click chuột vào một process bất kì
  onSelectedStepIdChanged(stepId: string | null) {
    this.selectedStepId = stepId;
    let obj: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY()
    for (let item of this.definition.sequence) {
      if (item['type'] == 'process' || item['type'] == 'group') {
        if (item['id'] == stepId) {
          obj = JSON.parse(JSON.stringify(item.properties.process)) as CM_PROCESS_ENTITY
          this.formData = new CM_PROCESS_ENTITY(obj);
          break;
        }
      }
      else if (this.onTrack(stepId, item)) {
        obj = JSON.parse(JSON.stringify(this.onTrack(stepId, item))) as CM_PROCESS_ENTITY
        this.formData = new CM_PROCESS_ENTITY(obj);
        break;
      }
    }
  }
  // #endregion Event select quy trình 

  // #region Event lưu quy trình

  saveInput(): void {
    if (!this.inputModel.id) {
      this.showLoading();
      this.processService.cM_PROCESS_LIST_Ins(this.inputModel)
        .pipe(finalize(() => this.hideLoading()))
        .subscribe((response) => {
          if (response.Result != '0') {
            this.showErrorMessage(response.ErrorDesc);
            return;
          }
          if (response['PROCESS_KEY']) {
            this.navigatePassParam('/app/admin/cm-process-concenter-edit', { id: response['PROCESS_KEY'] }, { filterInput: JSON.stringify(this.filterInput) });
            return;
          }
          this.addNewSuccess();
        });
    } else {
      //TODO - Begin Validation 
      if (this.stepList.find(step => !this.formValid(step.properties.process as CM_PROCESS_ENTITY, step.id))) {
        this.showErrorMessage('Lưu thất bại')
        return;
      }
      //TODO - End Validation 

      this.saveWorkFlow();
      this.showLoading();
      this.processService.cM_PROCESS_LIST_Upd(this.inputModel)
        .pipe(finalize(() => this.hideLoading()))
        .subscribe(response => {
          if (response.Result != '0') {
            this.showErrorMessage(response.ErrorDesc);
            return;
          }
          this.addNewSuccess();
        })
    }
  }

  // Tạo dữ liệu inputModel
  saveWorkFlow() {
    if (this.editPageState != EditPageState.edit) return;
    this.subProcessList = [];

    // break process into subprocess
    for (let step of this.stepList) {
      for (let pCond of step.properties.parCond as any) {
        const process = JSON.parse(JSON.stringify(step.properties.process)) as CM_PROCESS_ENTITY
        process.conditioN_STATUS = pCond.parent.toString()
        process.condition = pCond.condition.toString()

        let inputProcess: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY()
        inputProcess = Object.assign(inputProcess, process)
        this.subProcessList.push(inputProcess)
      }
    }

    // Process cuối cùng mặc định có status = 'D'
    // Thêm process phụ với condition status = 'D' để kết thúc
    for (let i = this.subProcessList.length - 1; i >= 1; i--) {
      if (!this.subProcessList.find(proc => proc.conditioN_STATUS == this.subProcessList[i].status)) {
        const doneProcess = {
          procesS_KEY: this.inputModel.procesS_KEY,
          grouP_STATUS: this.inputModel.grouP_STATUS_DONE,
          status: this.inputModel.grouP_STATUS_DONE,
          action: 'DONE',
          conditioN_STATUS: this.subProcessList[i].status,
          grouP_STATUS_NAME: this.inputModel.grouP_STATUS_DONE_NAME,
          description: this.l('Hoàn tất'),
          namE_ACTION: this.l('Hoàn tất'),
        } as CM_PROCESS_ENTITY

        let inputProcess: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY()
        inputProcess = Object.assign(inputProcess, doneProcess)

        this.subProcessList.push(inputProcess);
      }
    }

    // Thêm một process phụ với condition status = 'R' để từ chối
    const rejectProcess = {
      procesS_KEY: this.inputModel.procesS_KEY,
      grouP_STATUS: 'R',
      status: 'R',
      action: 'REJECT',
      conditioN_STATUS: null,
      grouP_STATUS_NAME: this.l('Từ chối'),
      description: this.l('Từ chối'),
      namE_ACTION: this.l('Từ chối'),
    } as CM_PROCESS_ENTITY

    let inputProcess: CM_PROCESS_ENTITY = new CM_PROCESS_ENTITY()
    inputProcess = Object.assign(inputProcess, rejectProcess)

    this.subProcessList.push(inputProcess)

    this.inputModel.lisT_PROCESS_ITEMS = this.subProcessList;
    this.inputModel.jsoN_WORKFLOW = this.definitionJSON;
  }
  // #endregion Event lưu quy trình

  formValid(form: CM_PROCESS_ENTITY, id: string): boolean {
    if (
      form.status == undefined ||
      form.action == undefined ||
      form.froM_STATUS == undefined ||
      form.namE_ACTION == undefined ||
      form.status == undefined ||
      !form.description
    ) {
      this.onSelectedStepIdChanged(id)
      this.showErrorMessage('Điền tất cả các mục có đánh dấu *');
      return false;
    }
    return true;
  }

  joinNameStatus(items: any[]): string {
    return items.map(item => item.parent.join(' - ')).join(' - ')
  }

  onIsToolboxCollapsedChanged(isCollapsed: boolean) {
    this.isToolboxCollapsed = isCollapsed;
  }

  onIsEditorCollapsedChanged(isCollapsed: boolean) {
    this.isEditorCollapsed = isCollapsed;
  }

  updateName(step: Step, event: Event, context: StepEditorContext) {
    step.name = (event.target as HTMLInputElement).value;
    context.notifyNameChanged();
  }

  updateRole(step: Step, role: string, context: StepEditorContext) {
    if (this.selectedStepId != step.id) return;
    context.notifyPropertiesChanged();
    step.name = (step.properties.process['role'] || 'Tất cả') + ' - ' + step.properties.process['action']
    context.notifyNameChanged();
  }

  updateAction(step: Step, action: string, context: StepEditorContext) {
    if (this.selectedStepId != step.id) return;
    context.notifyPropertiesChanged();
    step.name = (step.properties.process['role'] || 'Tất cả') + ' - ' + step.properties.process['action']
    context.notifyNameChanged();
  }

  updateContext(context: StepEditorContext) {
    context.notifyPropertiesChanged();
  }

  updateNameAction(step: Step, nameAction: string, context: StepEditorContext) {
    if (this.selectedStepId == step.id) {
      step.properties.process['namE_ACTION'] = nameAction
      context.notifyPropertiesChanged();
      context.notifyNameChanged();
    }
  }

  updateFromStatus(step: Step, fromStatus: string, context: StepEditorContext) {
    if (this.selectedStepId == step.id) {
      step.properties.process['froM_STATUS'] = fromStatus
      context.notifyPropertiesChanged();
      context.notifyNameChanged();
    }
  }

  updateDescription(step: Step, t: string, context: StepEditorContext) {
    if (this.selectedStepId == step.id) {
      step.properties.process['description'] = t
      context.notifyPropertiesChanged();
    }
  }

  updateField(field: string, value: string, step: Step, context: StepEditorContext) {
    if (this.selectedStepId == step.id) {
      step.properties.process[field] = value;
      context.notifyPropertiesChanged();
    }
  }

  updateRangeProcess(step: Step, rangeProcess: string, context: StepEditorContext) {
    if (this.selectedStepId == step.id) {
      step.properties.process['rangE_PROCESS'] = rangeProcess
      context.notifyPropertiesChanged();
    }
  }

  handleChangePrevProcess(event: CM_PROCESS_LIST_ENTITY) {
    this.inputModel.preV_GROUP_STATUS_DONE = event.grouP_STATUS_DONE;
    this.inputModel.preV_GROUP_STATUS_DONE_NAME = event.grouP_STATUS_DONE_NAME;
    this.inputModel.preV_PROCESS_NAME = event.procesS_NAME;
  }

  deleteStepBranch(step: Step, key: string, context: RootEditorContext | StepEditorContext) {
    if (Object.keys((step as any).branches).length == 1) {
      this.notify.warn('You cannot delete the last branch', 'Invalid')
      return
    }
    delete (step as any).branches[key];
    delete (step as any).properties.conditionsObj[key];
    (step as any).branches = this.reSortingStepBranch(step);
    (step as any).properties.conditionsObj = this.reSortingStepBranchCondition(step);
    context.notifyPropertiesChanged()
    this.designer.updateRootComponent()
  }

  reSortingStepBranch(step: Step) {
    let newObj = {};
    Object.keys((step as any).branches).map((keyOld, index) => {
      newObj[`Branch ${index + 1}`] = (step as any).branches[keyOld]
    })
    return newObj
  }

  reSortingStepBranchCondition(step: Step) {
    let newObj = {};
    Object.keys((step as any).properties.conditionsObj).map((keyOld, index) => {
      newObj[`Branch ${index + 1}`] = (step as any).properties.conditionsObj[keyOld]
    })
    return newObj
  }

  addStepBranch(step: Step, keyName: string, context: RootEditorContext | StepEditorContext) {
    let stepBranchLength = Object.keys((step as any).branches).length as any
    (step as any).branches[`${keyName} ${stepBranchLength + 1}`] = [];
    (step as any).properties.conditionsObj[`${keyName} ${stepBranchLength + 1}`] = ''
    context.notifyPropertiesChanged()
    this.designer.updateRootComponent()
  }

  renameKeys(obj, newKeys) {
    const keyValues = Object.keys(obj).map(key => {
      const newKey = newKeys[key] || key;
      return { [newKey]: obj[key] };
    });
    return Object.assign({}, ...keyValues);
  }

  updateStepBranchName(step: Step, key: string, event: Event, context: RootEditorContext | StepEditorContext) {
    const newKeys = { [key]: (event.target as HTMLInputElement).value };
    (step as any).branches = this.renameKeys((step as any).branches, newKeys)
    context.notifyPropertiesChanged()
    this.designer.updateRootComponent()
  }

  updateStepBranchCondition(step: Step, item: any, context: RootEditorContext | StepEditorContext) {
    (step as any).properties.conditionsObj[item.key] = item.value
    context.notifyPropertiesChanged()
    this.designer.updateRootComponent()
  }

  reloadDefinitionClicked() {
    this.definition = this.createDefinition();
    this.updateDefinitionJSON();
  }

  toggleReadonlyClicked() {
    this.isReadonly = !this.isReadonly;
  }

  toggleSelectedStepClicked() {
    if (this.selectedStepId) {
      this.selectedStepId = null;
    } else if (this.definition.sequence.length > 0) {
      this.selectedStepId = this.definition.sequence[0].id;
    }
  }

  toggleToolboxClicked() {
    this.isToolboxCollapsed = !this.isToolboxCollapsed;
  }

  toggleEditorClicked() {
    this.isEditorCollapsed = !this.isEditorCollapsed;
  }

  updateDefinitionJSON() {
    this.definitionJSON = JSON.stringify(this.definition, null, 2);
  }

  updateIsValid() {
    this.isValid = this.designer?.isValid();
  }

  loadWorkflow(): void {
    if (this.inputModel.jsoN_WORKFLOW) {
      try {
        this.onDefinitionChanged(JSON.parse(this.inputModel.jsoN_WORKFLOW))
      }
      catch {
        return
      }
    }
  }

  generateStatusList() {
    for (let i = 65; i <= 90; ++i)
      this.statusList.push({ name: String.fromCharCode(i) });
  }

  generateDefaultOrderList() {
    const orderTempList: { idx: number }[] = [];
    for (let i = 0; i <= 10; ++i) {
      this.orderList.push({ idx: i });
    }
    this.orderList = [...orderTempList];
  }

  generateOrderList(length: number, addAfter: number = 0) {
    const orderTempList: { idx: number }[] = [];
    for (let i = 0; i <= length + addAfter; i++) {
      orderTempList.push({ idx: i });
    }
    this.orderList = [...orderTempList];
  }

  goBack() {
    this.navigatePassParam('/app/admin/cm-process-concenter', null, { filterInput: JSON.stringify(this.filterInput) });
  }

  initIsApproveFunct(): void {
    this.ultilityService.isApproveFunct(this.getCurrentFunctionId()).subscribe((res) => {
      this.isApproveFunct = res;
    });
  }

  setFromIfOrderChanged(currentValue: number) {
    this.formData.from = currentValue == 0 ? null : `${currentValue - 1}`;
  }

  setStatusIfActionChanged(currentValue: string) {
    if (currentValue == 'REJECT') {
      this.formData.status = 'R';
    }
  }

  setNameActionandFromStatusIfActionChanged(currentValue: string) {
    switch (currentValue) {
      case 'CREATE':
        this.formData.namE_ACTION = 'Tạo phiếu';
        this.formData.froM_STATUS = 'ADD'
        break;
      case 'APPROVE':
        this.formData.namE_ACTION = 'Duyệt';
        this.formData.froM_STATUS = 'VIEW'
        break;
      case 'APPROVE_GROUP':
        this.formData.namE_ACTION = 'Nhóm duyệt';
        this.formData.froM_STATUS = 'VIEW'
        break;
      case 'ACCESS':
        this.formData.namE_ACTION = 'Xác nhận';
        this.formData.froM_STATUS = 'EDIT'
        break;
      case 'REJECT':
        this.formData.namE_ACTION = 'Trả về';
        this.formData.froM_STATUS = 'VIEW'
        break;
      default:
        this.formData.namE_ACTION = 'Tạo phiếu';
        this.formData.froM_STATUS = 'ADD'
    }
  }

  setValueIfActionChanged(currentValue: string) {
    this.setNameActionandFromStatusIfActionChanged(currentValue);

  }

  setCondStatusIfStatusChanged(currentValue: string) {
    if (currentValue === 'A')
      this.formData.conditioN_STATUS = null;
    else
      this.formData.conditioN_STATUS = String.fromCharCode(currentValue.charCodeAt(0) - 1);
  }

  onSave(): void {
    this.saveInput();
  }

  onReject(item: CM_PROCESS_LIST_ENTITY): void { }
  onSendApp(item: CM_PROCESS_LIST_ENTITY): void { }
  onSearch(): void { }
  onAdd(): void { }
  onDelete(item: CM_PROCESS_LIST_ENTITY): void { }
  onUpdate(item: CM_PROCESS_LIST_ENTITY): void { }
  onApprove(item: CM_PROCESS_LIST_ENTITY): void { }
  onViewDetail(item: CM_PROCESS_LIST_ENTITY): void { }
  onResetSearch(): void { }
}