import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ApexAxisChartSeries, ApexChart, ApexDataLabels, ApexFill, ApexGrid, ApexLegend, ApexPlotOptions, ApexStroke, ApexTooltip, ApexXAxis, ApexYAxis, ChartComponent, ChartType } from 'ng-apexcharts';
import { Chart } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { CM_BRANCH_ENTITY } from '@shared/service-proxies/service-proxies';
import { ListComponentBase } from '@app/utilities/list-component-base';
import { ComponentBase } from '@app/utilities/component-base';

export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  dataLabels: ApexDataLabels;
  plotOptions: ApexPlotOptions;
  yaxis: ApexYAxis;
  xaxis: ApexXAxis;
  fill: ApexFill;
  tooltip: ApexTooltip;
  stroke: ApexStroke;
  legend: ApexLegend;
  grid?: ApexGrid;
  labels?: []
};

export interface Options {
  id: string | number,
  title?: string,
  subTitle?: string,
  buttons?: string[],
  type: ChartType,
  showDetailOnChart: boolean
  labels: [],
  xaxis: [],
  colors: string[],
  position?: "top" | "right" | "bottom" | "left";
  middleLabel?: string
}

@Component({
  selector: 'chart-core',
  templateUrl: './chart-core.component.html',
  styleUrl: './chart-core.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChartCoreComponent {
  @Input() dataChart: ApexAxisChartSeries
  @Input() colors: string[]
  @Input() options: Options

  @Output() selectMilestone: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild("chart") chart: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  public chartPlugins = [ChartDataLabels];
  NgChartJs: any

  selectedDropdownValue: string; // Lưu trữ giá trị được chọn từ dropdown


  constructor() { }

  ngOnInit() {
    this.configApexChart()
    this.configChartJS()
    // this.centerText(this.options.middleLabel)
  }

  onDropdownValueChange(value: string) {
    console.log('Selected Dropdown Value:' + this.selectedDropdownValue + ', value: ' + value);

    // if (value !== this.selectedDropdownValue) { 
    // this.selectedDropdownValue = value;
    this.selectMilestone.emit({ event: 'dropdownChange', value });
    // }
  }

  selectMilestoneOutput(event) {
    this.selectMilestone.emit({ event, id: this.options.id || this.options.type })
  }

  centerText(label?: any) {
    console.log(label);
    if (label) {
      const centerTextPlugin = {
        id: 'centerText',
        beforeDraw(chart: any) {
          const { ctx, chartArea: { width, height } } = chart;
          const fontSize = (height / 114).toFixed(2);
          ctx.font = fontSize + "em sans-serif";
          ctx.textBaseline = "middle";
          ctx.fillStyle = "black";
          const text = label;
          const textX = Math.round((width - ctx.measureText(text).width) / 2);
          const textY = height / 2;
          ctx.fillText(text, textX, textY);
        }
      };
      Chart.register(centerTextPlugin);
    }
  }

  configChartJS() {
    this.NgChartJs = {
      type: this.options.type,
      data: {
        labels: this.options.labels,
        datasets: [
          {
            data: this.dataChart,
            backgroundColor: this.options.colors,
            hoverBackgroundColor: this.options.colors,
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: this.options.position || 'bottom',
            display: true,
            labels: {
              usePointStyle: true,
              pointStyle: 'rectRounded',
              boxWidth: 10,
              padding: 20,  // Giảm padding giữa legend và chart
            }
          },
          datalabels: {
            display: true,
            formatter: (value, context) => {
              return value + '%';
            },
            font: {
              size: 14,
            },
            color: '#FFFFFF',
          },
          title: {
            display: true,
            padding: {
              bottom: 10, // Giảm khoảng cách dưới title
            },
          },
          id: this.options.id || this.options.type,
          centerTextPlugin: {}

        },
      },
    };
  }

  configApexChart() {
    this.chartOptions = {
      series: this.dataChart,
      chart: {
        type: this.options.type,
        height: 340,
        zoom: {
          enabled: false
        },
      },
      plotOptions: {
        bar: {
          horizontal: false,
          columnWidth: "55%",
        },
        polarArea: {
          rings: {
            strokeWidth: 0
          },
          spokes: {
            strokeWidth: 0
          }
        },
        area: {
          fillTo: 'end'
        }
      },

      dataLabels: {
        enabled: this.options.showDetailOnChart,
      },
      stroke: {
        show: true,
        width: 2,
        curve: "smooth",
      },
      xaxis: {
        categories: this.options.xaxis || [
          "Feb",
          "Mar",
          "Apr",
          "May",
        ]
      },
      yaxis: {
        title: {
          text: "Xe (Số lượng)"
        },
        min: 0
      },
      fill: {
        opacity: 1,
        colors: this.colors

      },
      legend: {
        show: true,
        showForSingleSeries: true,
        markers: {

        },
        onItemClick: {
          toggleDataSeries: true
        },
        labels: {
          useSeriesColors: true
        },
        position: this.options.position || 'bottom',
      },
      labels: this.options.labels,

    };
  }

  borderRadiusChartJs() {
    const legendBorderRadiusPlugin = {
      id: 'legendBorderRadius',
      beforeDraw(chart: any) {
        if (chart.options.plugins.legend) {
          const ctx = chart.ctx;
          const legend = chart.legend;
          const borderRadius = 3; // Border radius in pixels

          legend.legendItems.forEach((item: any) => {
            const { x, y, width, height } = item;

            // Draw the legend box with rounded corners
            ctx.save();
            ctx.beginPath();
            ctx.moveTo(x + borderRadius, y);
            ctx.lineTo(x + width - borderRadius, y);
            ctx.arc(x + width - borderRadius, y + borderRadius, borderRadius, -Math.PI / 2, 0);
            ctx.lineTo(x + width, y + height - borderRadius);
            ctx.arc(x + width - borderRadius, y + height - borderRadius, borderRadius, 0, Math.PI / 2);
            ctx.lineTo(x + borderRadius, y + height);
            ctx.arc(x + borderRadius, y + height - borderRadius, borderRadius, Math.PI / 2, Math.PI);
            ctx.lineTo(x, y + borderRadius);
            ctx.arc(x + borderRadius, y + borderRadius, borderRadius, Math.PI, -Math.PI / 2);
            ctx.closePath();
            ctx.lineWidth = 1;
            ctx.strokeStyle = '#FFAB2D'; // Border color
            ctx.stroke();
            ctx.fillStyle = 'red'; // Background color
            ctx.fill();
            ctx.restore();
          });
        }
      }
    };

    Chart.register(legendBorderRadiusPlugin);

  }

}
