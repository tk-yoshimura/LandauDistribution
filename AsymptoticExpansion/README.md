# LandauDistribution - AsymptoticExpansion

## &lambda; &rarr; -&infin;

### Method
![asymp minus 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_minus_1.svg)  

![asymp minus 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_minus_2.svg)  
![asymp minus 3](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_minus_3.svg)  
![asymp minus 4](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_minus_4.svg)  

![asymp minus 6](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_minus_6.svg)  

![asymp minus 7](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_minus_7.svg)  

### Coefficients
![asymp minus 5](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_minus_5.svg)  

### a<sub>n</sub><sup>-</sup> Table
[Coef Generation(C#)](AsymptoticMinus)

|n|a<sub>n</sub><sup>-</sup>|
|----|----|
|0|1|
|1|1/24|
|2|-23/1152|
|3|11237/414720|
|4|-2482411/39813120|
|5|272785979/1337720832|
|6|-4175309343349/4815794995200|
|7|525035501918789/115579079884800|
|8|-628141988536245979/22191183337881600|
|9|53917386529177385523923/263631258054033408000|
|10|-148934765720971351352763767/88580102706155225088000|
|11|428338546734334777277256756263/27636992044320430227456000|
|12|-6301150244751080741665843707891149/39797268543821419527536640000|
|13|1695881990125518108674571524660426383/955134445051714068660879360000|
|14|-994159279221093204357661985843042030351/45846453362482275295722209280000|
|15|255772100763923957700402250028181246437052593/892755482749427578940336111616000000|
|16|-15353805610229212068528288410308574108746968171557/3770999159133582093443979735465984000000|

## &lambda; &rarr; &infin;

### Method
![asymp plus 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_plus_1.svg)  
![asymp plus 5](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_plus_5.svg)  
![asymp plus 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_plus_2.svg)  

### Coefficients

![asymp plus 3](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_plus_3.svg)  
![asymp plus 4](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_plus_4.svg)  
![asymp plus 6](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_plus_6.svg)  

### Solving **&omega;(&lambda;)**
![asymp plus 7](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_plus_7.svg)  

### Result
![asymp plus result](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/asymp_plus_result.svg)  

### a<sub>n</sub><sup>+</sup> Table
[Coef Generation(Maxima)](asymp_plus.wxmx)  
[Coef Generation(Maxima-batchfile)](asymp_plus.mxbat)

|n|a<sub>n</sub><sup>+</sup>|n|a<sub>n</sub><sup>+</sup>|
|----|----|----|----|
|0|1|||
|1|-1.8455686701969342787869758198352e0|17|-7.473037064232904501153754770809e10|
|2|-4.2846407430383847531337500817788e0|18|2.5073900240223071762995962264081e11|
|3|4.1988388606748058522382804473143e1|19|-1.4000645356418152010175124708595e11|
|4|-1.501425422434703431187317449966e2|20|-5.3842165777860629184398218793877e12|
|5|1.6971436527143008075893638967026e2|21|4.8989290125692282955358423097121e13|
|6|1.5308616756114047472784342097796e3|22|-2.713615081124373121084075045711e14|
|7|-1.2533161673697188765231718497799e4|23|9.9130304409439902548606801414022e14|
|8|5.2275779467011993580700329545325e4|24|-1.0753726745675913138969410391902e15|
|9|-1.038430735006061411019104023744e5|25|-1.843135861393738990149631403431e16|
|10|-3.5251556573222108643855810474626e5|26|1.9338031513325874276553577223558e17|
|11|4.7135194794390860194285790983463e6|27|-1.1917849406057678904496051676919e18|
|12|-2.6652557526212509771445798686047e7|28|5.0687431034591412775361253078221e18|
|13|9.242071471131500177077813575708e7|29|-1.0765430037665388507998356013818e19|
|14|-8.6050618329570458836691382506984e7|30|-5.2748784709128851529956479050806e19|
|15|-1.5235410325018326969458736793947e9|31|8.0803613728214172463710232272934e20|
|16|1.407099073481079449657382577173e10|32|-5.8667789882105999385398456507654e21|

## Reference
[K.S. Kölbig and B. Schorr, "Asymptotic expansions for the Landau density and distribution functions" (1983)](https://www.sciencedirect.com/science/article/abs/pii/0010465584900651)  
<sup>The lack of accuracy after term 4 of the &Phi; function is likely due to the fact that the zeta function is not evaluated with odd integers.</sup>