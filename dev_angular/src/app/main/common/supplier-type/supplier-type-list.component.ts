import {
    Injector,
    Component,
    OnInit,
    ViewChild,
    ViewEncapsulation,
    AfterViewInit,
    ChangeDetectorRef,
} from '@angular/core';
import {
    SupplierTypeServiceProxy,
    CM_SUPPLIER_TYPE_ENTITY,
    ReportInfo,
    AsposeServiceProxy,
    CM_REGION_ENTITY,
    RegionServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { LazyLoadEvent } from 'primeng/api';
import { IUiAction } from '@app/utilities/ui-action';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { finalize } from 'rxjs/operators';
import { ReportTypeConsts } from '@app/shared/core/utils/consts/ReportTypeConsts';
import { forkJoin } from 'rxjs';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LazyDropdownResponse } from '@app/shared/core/controls/dropdown-lazy-control/dropdown-lazy-control.component';

@Component({
    templateUrl: './supplier-type-list.component.html',
    styleUrls: ['./analysis.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class SupplierTypeListComponent extends AppComponentBase implements OnInit {
    // filterInput: CM_SUPPLIER_TYPE_ENTITY = new CM_SUPPLIER_TYPE_ENTITY();
    filterInput: CM_REGION_ENTITY = new CM_REGION_ENTITY();
    optionsChart: any;
    dataChart: any;
    dataChartByYear: any;
    dataChartOverview: any;
    dataChartRatio: any;
    dataDensity: any;
    regionInput: any;
    isProcessing: boolean = false;  // Flag để tránh vòng lặp
    isFirstCall: boolean = true;

    imgPath = '../../../../assets/icons/';
    // Dữ liệu cho các biểu đồ
    constructor(
        injector: Injector,
        private fileDownloadService: FileDownloadService,
        private asposeService: AsposeServiceProxy,
        private changeDetector: ChangeDetectorRef,
        private supplierTypeService: SupplierTypeServiceProxy,
        private regionService: RegionServiceProxy,
    ) {
        super(injector);

        this.dataChart = [];
        this.regionInput = 'Vx Lăng Cha Cả';
        this.dataChartByYear = [];
        this.dataChartOverview = [];
        this.dataChartRatio = [];
        this.dataDensity = [];
    }


    assCollectMultiDtOptions: {};

    formatVND(money: number) {
        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(money);
    }

    getMilestone(event) {
        console.log('Milestone Event:', event);

        // Nếu event chứa thông tin về giá trị dropdown, cập nhật yearInput
        if (event && event.value && event.value !== this.regionInput) {
            this.regionInput = event.value;
            console.log('Updated region:', this.regionInput);

            // Đánh dấu là đang xử lý
            this.isProcessing = true;

            // Gọi lại loadDataFromDatabase để tải dữ liệu mới
            // this.handleDropdownChange(event)

            // Đánh dấu là đã xử lý xong
            this.isProcessing = false;
        } else {
            console.log('Year input is the same, no need to update');
        }
    }


    ngOnInit(): void {
        this.initAnalysis();
    }

    ngAfterViewInit(): void {
        this.changeDetector.detectChanges();
    }


    processDataForChart(data: any[]) {
        const years = [...new Set(data.map(item => item.avgYear))]; // Danh sách các năm
        const categories = [...new Set(data.map(item => item.avgCategoryName))]; // Danh sách loại phương tiện

        const datasets = categories.map(category => {
            const dataPoints = years.map(year => {
                const record = data.find(item => item.avgYear === year && item.avgCategoryName === category);
                return record ? record.avgVehicleCount : 0; // Nếu không có dữ liệu, trả về 0
            });

            return {
                name: category,
                data: dataPoints,
                borderColor: category === 'Ô tô' ? 'red' : 'green', // Tùy chỉnh màu sắc
                backgroundColor: category === 'Ô tô' ? 'rgba(54, 235, 60, 0.5)' : 'rgba(203, 204, 110, 0.95)',
            };
        });
        return { labels: years, datasets };
    };

    initAnalysis() {
        forkJoin({
            dataOverview: this.supplierTypeService.vehicleDetection_View_ByRegion(this.regionInput),
            dataByYear: this.supplierTypeService.vehicleStatistic_OfRegion_AvgByYear(this.regionInput),
            dataRatio: this.supplierTypeService.vehicleDetection_Ratio_ByRegion(this.regionInput),
            dataDensity: this.supplierTypeService.vehicleDetection_PeakYear_ByRegion(this.regionInput)
            // google: this.dashboardService.dB_STATUS_ASSET_QUANTITY_PIE(this.body)
        }).subscribe({
            next: ({ dataByYear, dataOverview, dataRatio, dataDensity }) => {
                console.log('step 2: Data fetched from both APIs');

                // Xử lý dữ liệu từ API đầu tiên
                if (Array.isArray(dataByYear)) {
                    console.log('Data Input:', dataByYear);
                    const processedData = this.processDataForChart(dataByYear);
                    console.log('Processed Data:', processedData);
                    this.dataChartByYear = {
                        title: 'Biến động số lượng phương tiện qua các năm',
                        subTitle: 'Dữ liệu thống kê trung bình',
                        xaxis: processedData.labels,
                        type: 'line',
                        position: 'bottom',
                        data: {
                            labels: processedData.labels,
                            datasets: processedData.datasets,
                        },
                    }
                };


                const roundedDataRatio = dataRatio.map(item => ({
                    ...item,
                    ratioPercentage: parseFloat(item.ratioPercentage.toFixed(2))
                }));

                if (Array.isArray(dataRatio)) {
                    this.dataChartRatio = {
                        title: 'Tỷ lệ phương tiện',
                        subTitle: 'Tỉ lệ các loại phương tiện',
                        colors: ['#FF4D4D ', '#E6B800  '],
                        type: 'pie',
                        showDetailOnChart: true,
                        middleLabel: '110%',
                        labels: dataRatio.map(item => item.ratioCaregoryName),
                        // position: 'bottom',
                        data: {
                            datasets: roundedDataRatio.map(item => item.ratioPercentage), // [17.29, 82.70]                                     
                        },

                    };
                    // console.log('dataChartRatio: ', this.dataChartRatio.data);
                }

                // Kết hợp dữ liệu
                this.combineData();

                this.dataDensity = Array.isArray(dataDensity) && dataDensity.length > 0 ? dataDensity[0] : null;

                // if (Array.isArray(dataOverview) && dataOverview.length > 0) {
                //     this.regionInput = dataOverview[0].regionName || 'Khu vực chưa xác định';
                // }
            },
            error: (err) => {
                console.error('Lỗi khi lấy dữ liệu:', err);
                this.notify.error('Lấy dữ liệu thất bại');
            }
        });
    }

    loadDataFromDatabase() {
        // console.log('yearInput: ' + this.yearInput);
        this.supplierTypeService.vehicleDetection_View_ByRegion(this.regionInput).subscribe({
            next: (response: any) => {
                console.log('loadDataFromDatabase True');
                // Kiểm tra nếu response là mảng
                if (Array.isArray(response)) {
                    // Khởi tạo dữ liệu cho chart
                    const motorbikeDataByYear = [];
                    const carDataByYear = [];
                    const areaLabelsByYear = [];

                    console.log('dataChartByYear in loadFromDatabase: ', this.dataChartByYear);
                    console.log('dataChartOverview in loadFromDatabase: ', this.dataChartOverview);
                    this.dataChart = [this.dataChartByYear, this.dataChartRatio];
                } else {
                    this.notify.error('Dữ liệu không hợp lệ hoặc không có dữ liệu.');
                }
            },

            error: (err) => {
                this.notify.error('Lấy dữ liệu thất bại');
            }
        });
    };
    combineData() {
        if (this.dataChartByYear && this.dataChartRatio) {
            // Kết hợp dữ liệu từ cả hai biểu đồ vào mảng chính
            console.log("dataChartByYear:", this.dataChartByYear);
            console.log("dataChartOverview:", this.dataChartOverview);
            this.dataChart = [this.dataChartByYear, this.dataChartRatio];
            console.log('dataChart: ', this.dataChart);
        }
    }

    getAllMenu(data?: LazyDropdownResponse): void {
        const filterInput: CM_REGION_ENTITY = new CM_REGION_ENTITY();
        filterInput.skipCount = data.state?.skipCount;
        filterInput.totalCount = data.state?.totalCount;
        filterInput.maxResultCount = data.state?.maxResultCount;
        filterInput.regioN_NAME = data.state?.filter;
        this.regionService.cM_REGION_Search(filterInput)
            .subscribe(result => {
                if (this.isFirstCall) {
                    this.isFirstCall = false;
                    var nullValue: any = {
                        regioN_CODE: ' ',
                        regioN_NAME: this.l('NullSelect')
                    };
                    result.items.unshift(nullValue);
                }
                data.callback(result)
            })
    }

    handleDropdownChange(selectedValue: any): void {
        // Xử lý logic khi một mục được chọn trong dropdown
        console.log('Selected Region Code:', selectedValue);

        // Ví dụ: Cập nhật dữ liệu khác dựa trên lựa chọn
        this.regionInput = selectedValue.regioN_NAME

        this.supplierTypeService.vehicleStatistic_OfRegion_AvgByYear(this.regionInput).subscribe({
            next: (response: any) => {
                if (Array.isArray(response)) {
                    console.log('Data Input:', response);
                    const processedData = this.processDataForChart(response);
                    console.log('Processed Data:', processedData);
                    this.dataChartByYear = {
                        title: 'Biến động số lượng phương tiện qua các năm',
                        subTitle: 'Dữ liệu thống kê trung bình',
                        xaxis: processedData.labels,
                        type: 'line',
                        position: 'bottom',
                        data: {
                            labels: processedData.labels,
                            datasets: processedData.datasets,
                        },
                    }
                    
                }
                else {
                    this.notify.error('Dữ liệu không hợp lệ hoặc không có dữ liệu.');
                }
            },

            error: (err) => {
                this.notify.error('Lấy dữ liệu thất bại');
            }
        });
        this.supplierTypeService.vehicleDetection_Ratio_ByRegion(this.regionInput).subscribe({
            next: (response: any) => {
                if (Array.isArray(response)) {
                    console.log('Data ratio:', response);
                    const roundedDataRatio = response.map(item => ({
                        ...item,
                        ratioPercentage: parseFloat(item.ratioPercentage.toFixed(2))
                    }));

                    this.dataChartRatio = {
                        title: 'Tỷ lệ phương tiện',
                        subTitle: 'Tỉ lệ các loại phương tiện',
                        colors: ['#FF4D4D ', '#E6B800  '],
                        type: 'pie',
                        showDetailOnChart: true,
                        middleLabel: '110%',
                        labels: response.map(item => item.ratioCaregoryName),
                        // position: 'bottom',
                        data: {
                            datasets: roundedDataRatio.map(item => item.ratioPercentage), // [17.29, 82.70]                                     
                        },
                    };
                }
                else {
                    this.notify.error('Dữ liệu không hợp lệ hoặc không có dữ liệu.');
                }
            },

            error: (err) => {
                this.notify.error('Lấy dữ liệu thất bại');
            }
        });
        
        this.supplierTypeService.vehicleDetection_PeakYear_ByRegion(this.regionInput).subscribe({
            next: (response: any) => {
                if (Array.isArray(response)) {
                    this.dataDensity = response.length > 0 ? response[0] : null;
                }
                else {
                    this.notify.error('Dữ liệu không hợp lệ hoặc không có dữ liệu.');
                }
            },

            error: (err) => {
                this.notify.error('Lấy dữ liệu thất bại');
            }
        });

        this.combineData();
    }


    onReject(item: CM_SUPPLIER_TYPE_ENTITY): void {
        throw new Error('Method not implemented.');
    }

    onSendApp(item: CM_SUPPLIER_TYPE_ENTITY): void {
        throw new Error('Method not implemented.');
    }
}
