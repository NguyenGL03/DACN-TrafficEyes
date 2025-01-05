import { Component, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DashboardCustomizationConst } from '@app/shared/common/customizable-dashboard/DashboardCustomizationConsts';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import ApexCharts from 'apexcharts';
import moment from 'moment';
import { DashboardServiceProxy, DB_VEHICLE_ENTITY } from '@shared/service-proxies/service-proxies';
import { forkJoin } from 'rxjs';
import { YEAR } from 'ngx-bootstrap/chronos/units/constants';
import { Legend } from 'chart.js';

@Component({
    templateUrl: './host-dashboard.component.html',
    styleUrls: ['./host-dashboard.component.less'],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})

export class HostDashboardComponent extends AppComponentBase {
    dashboardName = DashboardCustomizationConst.dashboardNames.defaultHostDashboard;
    optionsChart: any;
    dataChart: any;
    dataChartByYear: any;
    dataChartOverview: any;
    dataChartRatio: any;
    yearInput: any;
    isProcessing: boolean = false;  // Flag để tránh vòng lặp

    dataSummary: any
    imgPath = '../../../../assets/icons/'
    // showDashboard: boolean = false

    // dataChart: any[] = [];

    body: any = {}

    constructor(
        injector: Injector,
        private dashboardService: DashboardServiceProxy
    ) {
        super(injector);
        this.dataChart = [];
        this.yearInput = '2023';
        this.dataChartByYear = [];
        this.dataChartOverview = [];
        this.dataChartRatio = [];

        // this.dataChart = [
        //     {
        //         title: 'Số lượng phương tiện tại các khu vực',
        //         subTitle: '01/01/2014 - 03/12/2024',
        //         colors: ['#00BA2E', '#02771F'],
        //         xaxis: ['V.x Lăng Cha Cả', 'Lý Thái Tổ', 'V.x Dân Chủ', 'Xô Viết Nghệ Tĩnh', 'V.X Cộng Hòa', 'Cách mạng tháng Tám'],
        //         type: 'bar',
        //         data: {
        //             datasets: [
        //                 {
        //                     name: 'Xe máy',
        //                     data: [65, 59, 67, 81, 30, 40]
        //                 },
        //                 {
        //                     name: 'Ô tô',
        //                     data: [18, 86, 80, 19, 20, 45]
        //                 },
        //             ],
        //         },
        //     },
        // {
        //     title: 'Trung bình xe máy, ôtô qua các năm',
        //     subTitle: '01/01/2014 - 03/12/2024',
        //     colors: ['#00BA2E', '#02771F'],
        //     xaxis: ['V.x Lăng Cha Cả', 'Lý Thái Tổ', 'V.x Dân Chủ', 'Xô Viết Nghệ Tĩnh', 'V.X Cộng Hòa', 'Cách mạng tháng Tám'],
        //     type: 'bar',
        //     data: {
        //         datasets: [
        //             {
        //                 name: 'Xe máy',
        //                 data: [71.4, 59.7, 65.5, 79.5, 29.1, 44.3]
        //             },
        //             {
        //                 name: 'Ô tô',
        //                 data: [12.2, 42.1, 33.3, 15.4, 18, 30]
        //             },
        //         ],
        //     },
        // },
        // {
        //     title: 'Biến động giá trị mua sắm tài sản qua các năm',
        //     subTitle: '01/01/2024 - 31/12/2024',
        //     buttons: ['Các năm', 2024, 2023, 2022, 2021, 2020],
        //     xaxis: ['2024', '2023', '2022', '2021', '2020'],
        //     colors: ['#64A611', '#828282', '#B5B5B5', '#FFAB2D', '#ED1C24', '#3577DB', '#26346F'],
        //     labels: ['2024', '2023', '2022', '2021', '2020'],
        //     type: 'line',
        //     position: 'bottom',
        //     data: {
        //         labels: ['2024', '2023', '2022', '2021', '2020'],
        //         datasets: [
        //             {
        //                 name: '2024',
        //                 data: [800, 400, 500, 300, 600]
        //             }
        //         ],
        //     },
        // },
        // {
        //     title: 'Tỷ trọng giá trị theo loại tài sản',
        //     colors: ['#64A611', '#828282', '#B5B5B5', '#FFAB2D', '#ED1C24', '#3577DB', '#26346F'],
        //     type: 'pie',
        //     showDetailOnChart: true,
        //     middleLabel:'110%',
        //     labels: ['Thiết bị, dụng cụ quản lý', 'TSCĐ hữu hình', 'TSCĐ vô hình', 'Nhà cửa, vật kiến trúc', 'Máy móc, thiết bị', 'Phương tiện vận tải', 'Phương tiện vận tải'],
        //     position: 'right',
        //     data: {
        //         datasets: [10, 10, 23, 22, 10, 10, 15],
        //     },
        // },
        // {
        //     title: 'polarArea',
        //     colors: ['#3577DB', '#26346F', '#ED1C24', '#FFAB2D', '#64A611'],
        //     type: 'polarArea',
        //     showDetailOnChart: true,
        //     position: 'right',
        //     labels: ['Giá trị mua sắm chờ xử lý', 'Giá trị mua sắm đang xử lý', 'Giá trị mua sắm đã xử lý', 'Đã hoàn tất thanh toán nhà cung cấp', 'Giá trị tiết kiệm ngân sách'],
        //     data: {
        //         datasets: [18, 10, 20, 22, 30],
        //     }
        // },
        // {
        //     title: 'donut',
        //     colors: ['#ED1C24', '#26346F', '#3577DB', '#FFAB2D'],
        //     type: 'doughnut',
        //     showDetailOnChart: true,
        //     labels: ['Ngân sách chưa sử dụng', 'Ngân sách đang trình chủ trương sử dụng', 'Ngân sách đang quá trình mua sắm', 'Ngân sách đã hoàn tất mua sắm và thanh toán'],
        //     position: 'right',
        //     data: {
        //         datasets: [25, 13, 37, 25],
        //     },
        // },
        // ]

        this.dataSummary = [
            // {
            //     icon: 'vehicle.svg',
            //     title: 'Lưu lượng phương tiện',
            //     content: 200000,
            //     progress: 100,
            //     color: 'rgba(100, 166, 17, 1)'
            // },
            // {
            //     icon: 'average.svg',
            //     title: 'Lưu lượng trung bình',
            //     content: 58,
            //     color: 'rgba(255, 171, 45, 0.8)'
            // },
            // {
            //     icon: 'motorcycle.svg',
            //     title: 'Lưu lượng trung bình xe máy',
            //     content: 75,
            //     progress: 65,
            //     color: 'rgba(238 237 47)'
            // },
            // {
            //     icon: 'car.svg',
            //     title: 'Lưu lượng trung bình ô tô',
            //     content: 20,
            //     progress: 40,
            //     color: 'rgba(237, 28, 36, 0.8)'
            // },
        ]

        this.body.useR_LOGIN = this.appSession.user.userName
        this.body.brancH_ID = this.appSession.user.branchCode
        this.body.creatE_DT = moment()
        this.body.xml = 'string'
        this.body.dashboarD_CHILDEN = [{
            year: "string",
            amorT_STATUS: "string",
            totaL_COUNT: 0,
            typE_ID: "string"
        }]

    }

    ngOnInit() {
        this.initDashboard();
        // console.log('step 1');

    }

    formatVehicleCount(count: number): string {
        return count.toLocaleString('vi-VN', { 
            useGrouping: true 
        }).replace(/\./g, ',') + ' xe';
    }

    getMilestone(event: any) {
        console.log('Milestone Event:', event);

        // Nếu event chứa thông tin về giá trị dropdown, cập nhật yearInput
        if (event && event.value && event.value !== this.yearInput) {
            this.yearInput = event.value;
            console.log('Updated Year:', this.yearInput);

            // Đánh dấu là đang xử lý
            this.isProcessing = true;

            // Gọi lại loadDataFromDatabase để tải dữ liệu mới
            this.loadDataFromDatabase();

            // Đánh dấu là đã xử lý xong
            this.isProcessing = false;
        } else {
            console.log('Year input is the same, no need to update');
        }
    }

    initDashboard() {
        forkJoin({
            dataByYear: this.dashboardService.vehicleStatisticsByYear_Top8(this.yearInput),
            dataOverview: this.dashboardService.vehicleStatistics_Top8(),
            dataRatio: this.dashboardService.vehicleDetection_CategoryRatio(),
            dataSummary: this.dashboardService.vehicleDashboard_Summary(),
            // google: this.dashboardService.dB_STATUS_ASSET_QUANTITY_PIE(this.body)
        }).subscribe({
            next: ({ dataByYear, dataOverview, dataRatio, dataSummary }) => {
                console.log('step 2: Data fetched from both APIs');

                // Xử lý dữ liệu từ API đầu tiên
                if (Array.isArray(dataByYear)) {
                    this.dataChartByYear = {
                        title: 'Số lượng phương tiện cao nhất các khu vực',
                        subTitle: 'Dữ liệu thống kê trung bình',
                        colors: ['#00BA2E', '#02771F'],
                        xaxis: dataByYear.map(item => item.regionNameByYear),
                        type: 'bar',
                        data: {
                            datasets: [
                                { name: 'Xe máy', data: dataByYear.map(item => item.avgMotorbikeCountByYear) },
                                { name: 'Ô tô', data: dataByYear.map(item => item.avgCarCountByYear) }
                            ]
                        }
                    };
                }

                // Xử lý dữ liệu từ API thứ hai
                if (Array.isArray(dataOverview)) {
                    this.dataChartOverview = {
                        title: 'Số lượng phương tiện tổng quan',
                        subTitle: 'Dữ liệu thống kê trung bình',
                        colors: ['#00BA2E', '#02771F'],
                        xaxis: dataOverview.map(item => item.regionName),
                        type: 'bar',
                        data: {
                            datasets: [
                                { name: 'Xe máy', data: dataOverview.map(item => item.avgMotorbikeCount) },
                                { name: 'Ô tô', data: dataOverview.map(item => item.avgCarCount) }
                            ]
                        }
                    };
                }

                const roundedDataRatio = dataRatio.map(item => ({
                    ...item,
                    percentage: parseFloat(item.percentage.toFixed(2))
                }));

                if (Array.isArray(dataRatio)) {
                    this.dataChartRatio = {
                        title: 'Tỷ lệ phương tiện',
                        subTitle: 'Tỉ lệ các loại phương tiện',
                        colors: ['#FF4D4D ', '#E6B800  '],
                        type: 'pie',
                        showDetailOnChart: true,
                        middleLabel: '110%',
                        labels: dataRatio.map(item => item.categoryName),
                        // position: 'bottom',
                        data: {
                            datasets: roundedDataRatio.map(item => item.percentage), // [17.29, 82.70]                                     
                        },
                    
                    };
                    // console.log('dataChartRatio: ', this.dataChartRatio.data);
                }

                // Kết hợp dữ liệu
                this.combineData();

                if (Array.isArray(dataSummary)) {
                    this.dataSummary = [
                        {
                            icon: 'vehicle.svg',
                            title: 'Tổng lượng phương tiện',
                            content: dataSummary.map(item => item.totalVehicles),
                            // progress: 100,
                            color: 'rgba(100, 166, 17, 1)'
                        },
                        {
                            icon: 'topRegion.svg',
                            title: 'Khu vực đông đúc',
                            content: dataSummary.map(item => item.topRegion),
                            color: 'rgba(255, 171, 45, 0.8)'
                        },
                        {
                            icon: 'average.svg',
                            title: 'Mật độ trung bình',
                            content: dataSummary.map(item => item.averageVehicleDensity),
                            // progress: 65,
                            color: 'rgba(238 237 47)'
                        },
                        {
                            icon: 'peakYear.svg',
                            title: 'Năm cao điểm',
                            content: dataSummary.map(item => item.peakYear),
                            // progress: 40,
                            color: 'rgba(237, 28, 36, 0.8)'
                        },
                    ]
                }


            },
            error: (err) => {
                console.error('Lỗi khi lấy dữ liệu:', err);
                this.notify.error('Lấy dữ liệu thất bại');
            }
        });
    }


    loadDataFromDatabase() {
        // console.log('yearInput: ' + this.yearInput);
        this.dashboardService.vehicleStatisticsByYear_Top8(this.yearInput).subscribe({
            next: (response: any) => {
                console.log('loadDataFromDatabase True');
                // Kiểm tra nếu response là mảng
                if (Array.isArray(response)) {
                    // Khởi tạo dữ liệu cho chart
                    const motorbikeDataByYear = [];
                    const carDataByYear = [];
                    const areaLabelsByYear = [];

                    // Duyệt qua dữ liệu để lấy thông tin cho biểu đồ
                    response.forEach((item: DB_VEHICLE_ENTITY) => {
                        motorbikeDataByYear.push(item.avgMotorbikeCountByYear);
                        carDataByYear.push(item.avgCarCountByYear);
                        areaLabelsByYear.push(item.regionNameByYear);
                    });

                    // Cập nhật dữ liệu cho biểu đồ
                    this.dataChartByYear =
                    {
                        title: 'Số lượng phương tiện cao nhất các khu vực',
                        subTitle: 'Dữ liệu thống kê trung bình',
                        colors: ['#00BA2E', '#02771F'],  // Màu sắc của biểu đồ
                        xaxis: areaLabelsByYear,  // Các khu vực trên trục x
                        type: 'bar',  // Loại biểu đồ
                        data: {
                            datasets: [
                                {
                                    name: 'Xe máy',
                                    data: motorbikeDataByYear  // Dữ liệu xe máy
                                },
                                {
                                    name: 'Ô tô',
                                    data: carDataByYear  // Dữ liệu ô tô
                                },
                            ],
                        },
                    };
                    console.log('dataChartByYear in loadFromDatabase: ', this.dataChartByYear);
                    console.log('dataChartOverview in loadFromDatabase: ', this.dataChartOverview);
                    this.dataChart = [this.dataChartByYear, this.dataChartRatio, this.dataChartOverview];
                } else {
                    this.notify.error('Dữ liệu không hợp lệ hoặc không có dữ liệu.');
                }
            },

            error: (err) => {
                this.notify.error('Lấy dữ liệu thất bại');
            }
        });
    }
    // loadDataFromDatabase1(){
    //     this.dashboardService.vehicleStatistics_Top8().subscribe({
    //         next: (response: any) => {
    //             // Kiểm tra nếu response là mảng
    //             if (Array.isArray(response)) {
    //                 // Khởi tạo dữ liệu cho chart
    //                 const motorbikeData = [];
    //                 const carData = [];
    //                 const areaLabels = [];

    //                 // Duyệt qua dữ liệu để lấy thông tin cho biểu đồ
    //                 response.forEach((item: DB_VEHICLE_ENTITY) => {
    //                     motorbikeData.push(item.avgMotorbikeCount);
    //                     carData.push(item.avgCarCount);
    //                     areaLabels.push(item.regionName);
    //                 });

    //                 // Cập nhật dữ liệu cho biểu đồ
    //                 this.dataChartOverview  = [
    //                     {
    //                         title: 'Số lượng phương tiện tổng quan',
    //                         subTitle: 'Dữ liệu thống kê trung bình',
    //                         colors: ['#00BA2E', '#02771F'],  // Màu sắc của biểu đồ
    //                         xaxis: areaLabels,  // Các khu vực trên trục x
    //                         type: 'bar',  // Loại biểu đồ
    //                         data: {
    //                             datasets: [
    //                                 {
    //                                     name: 'Xe máy',
    //                                     data: motorbikeData  // Dữ liệu xe máy
    //                                 },
    //                                 {
    //                                     name: 'Ô tô',
    //                                     data: carData  // Dữ liệu ô tô
    //                                 },
    //                             ],
    //                         },
    //                     },
    //                 ];

    //             } else {
    //                 this.notify.error('Dữ liệu không hợp lệ hoặc không có dữ liệu.');
    //             }
    //         },

    //         error: (err) => {
    //             this.notify.error('Lấy dữ liệu tổng quan thất bại');
    //         }
    //     });
    // }
    combineData() {
        if (this.dataChartByYear && this.dataChartOverview) {
            // Kết hợp dữ liệu từ cả hai biểu đồ vào mảng chính
            console.log("dataChartByYear:", this.dataChartByYear);
            console.log("dataChartOverview:", this.dataChartOverview);
            this.dataChart = [this.dataChartByYear, this.dataChartRatio, this.dataChartOverview];
            console.log('dataChart: ', this.dataChart);
        }
    }
};



// next: (response: any) => {
//     console.log('Dữ liệu từ API:', response);  // Kiểm tra dữ liệu trả về
//     if (Array.isArray(response)) {
//         // Chuyển đổi dữ liệu theo định dạng phù hợp với ApexCharts
//         this.dataChart = response.map((item: DB_VEHICLE_ENTITY) => ({
//             title: `Lưu lượng phương tiện tại ${item.area}`,
//             subTitle: 'Dữ liệu thống kê trung bình',
//             colors: ['#00BA2E', '#02771F'],  // Màu sắc của biểu đồ
//             xaxis: [item.area],  // Khu vực
//             type: 'bar',  // Loại biểu đồ
//             data: {
//                 datasets: [
//                     {
//                         name: 'Xe máy',
//                         data: [item.avgMotorbikeQuantity]  // Số lượng xe máy
//                     },
//                     {
//                         name: 'Ô tô',
//                         data: [item.avgCarQuantity]  // Số lượng ô tô
//                     },
//                 ],
//             },
//         }));
//     } else {
//         this.notify.error('Dữ liệu không hợp lệ hoặc không có dữ liệu.');
//     }
// },