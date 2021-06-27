# LandauDistribution

In probability theory, the Landau distribution is a probability distribution named after Lev Landau.  
Because of the distribution's "fat" tail, the moments of the distribution,  
like mean or variance, are undefined. The distribution is a particular case of stable distribution.

## Definition

The original Landau distribution defined by Landau can be evaluated on real numbers as follows:

![define origin](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_origin.svg)

The Landau distribution, generalized to a stable distribution by introducing position and scale parameters, is as follows:

![define stabledist](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_stabledist_generalized.svg)

The relevance of the original definition is as follows:

![define relevance](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_relevance.svg)

![define relevance 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/define_relevance_2.svg)

![pdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/pdf.svg)  
![logpdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/logpdf.svg)  
![cdf](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/cdf.svg)  

## Property of Tail

The behavior at the tail is as follows:

![tail largex](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_largex.svg)  
![tail largex approx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_largex_approx.svg)  

![tail lessx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_lessx.svg)  
![tail lessx approx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/tail_lessx_approx.svg)

## Numeric Integration

Since the convergence is different, it is necessary to use different equations for positive and negative x.

### x &geq; 0

Since p(x) has periodicity due to sin, this is used.

![integrad px](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrand_px.svg)  

![integrate px 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_1.svg)  
![integrate px 2](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_2.svg)  

If x is positive, the integral value is always positive.

![integrate px 3](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_3.svg)  

Evaluate the peak point of the integrated function. It can be seen that t &lt; 1 or less always.

![integrate px 4](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_4.svg)  

Evaluate the upper bound.

![integrate px 5](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_5.svg)  
![integrate px 6](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_px_6.svg)  

### x &lt; 0

Use an equation with small oscillations whose absolute value is not greater than 1 for negative x.  
However, the period is not fixed.  
Furthermore, for negative x, the value of the probability density function decreases rapidly,   
allowing the calculation to be terminated before the oscillations become too severe.

![integrad nx](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrand_nx.svg)  

![integrate nx 1](https://github.com/tk-yoshimura/LandauDistribution/blob/main/figures/integrate_nx_1.svg)  

## WolframAlpha Reference

[WolframAlpha](https://www.wolframalpha.com/)

input:  
N&#91;PDF&#91;LandauDistribution&#91;0, pi/2&#93;, *x*&#93;, 20&#93;  
N&#91;CDF&#91;LandauDistribution&#91;0, pi/2&#93;, *x*&#93;, 20&#93;  
N&#91;1 - CDF&#91;LandauDistribution&#91;0, pi/2&#93;, *x*&#93;, 20&#93;

|x|pdf|cdf|1-cdf|
|----|----|----|----|
|-8|&asymp;0|&asymp;0|&asymp;1|
|-7.5|1.3963580741658513783e-288|2.0977644853283045445e-291|&asymp;1|
|-7|4.9749117858699009892e-175|1.2316337907401396416e-177|&asymp;1|
|-6.5|3.3646333457816195657e-106|1.3722551000771884481e-108|&asymp;1|
|-6|1.7051074447851705700e-64|1.1450581306161646013e-66|&asymp;1|
|-5.5|3.0502060150089809970e-39|3.3699400957557151075e-41|&asymp;1|
|-5|5.7299682705730103032e-24|1.0401083461973864142e-25|&asymp;1|
|-4.5|9.5412628518507043281e-15|2.8394470726841722797e-16|9.9999999999999971606e-1|
|-4|3.3899538635808424464e-9|1.6484018900019493678e-10|9.9999999983515981100e-1|
|-3.75|2.5449478726175362333e-7|1.5790361960475750996e-8|9.9999998420963803952e-1|
|-3.5|7.1518498698243676772e-6|5.6532832980752883469e-7|9.9999943467167019247e-1|
|-3.25|9.3505770508751683307e-5|9.3988454165070266140e-6|9.9999060115458349297e-1|
|-3|6.7372861390806443293e-4|8.5921266252089708850e-5|9.9991407873374791029e-1|
|-2.75|3.0524170678404778510e-3|4.9259520485740286333e-4|9.9950740479514259714e-1|
|-2.5|9.6369248034230374967e-3|1.9619094349325484410e-3|9.9803809056506745156e-1|
|-2.25|2.2968463042829752952e-2|5.8780700091531217050e-3|9.9412192999084687830e-1|
|-2|4.3985478379363886601e-2|1.4094357934148776054e-2|9.8590564206585122395e-1|
|-1.875|5.6958412565992413813e-2|2.0389258154529440097e-2|9.7961074184547055990e-1|
|-1.75|7.1052095303906114372e-2|2.8380772808181761103e-2|9.7161922719181823890e-1|
|-1.625|8.5760503634986165690e-2|3.8177931966667932412e-2|9.6182206803333206759e-1|
|-1.5|1.0055075172901865837e-1|4.9824274974910163226e-2|9.5017572502508983677e-1|
|-1.375|1.1491286044707405962e-1|6.3297692801797284634e-2|9.3670230719820271537e-1|
|-1.25|1.2839849350344398806e-1|7.8515847611795490634e-2|9.2148415238820450937e-1|
|-1.125|1.4064598804419149227e-1|9.5345588745253049414e-2|9.0465441125474695059e-1|
|-1|1.5139191152148557738e-1|1.1361464105402330181e-1|8.8638535894597669819e-1|
|-0.875|1.6047127886631768423e-1|1.3312400044900388005e-1|8.6687599955099611995e-1|
|-0.75|1.6780946084066212368e-1|1.5365980392010946302e-1|8.4634019607989053698e-1|
|-0.625|1.7340892342201156848e-1|1.7500383368792738871e-1|8.2499616631207261129e-1|
|-0.5|1.7733354699252185004e-1|1.9694218692005420768e-1|8.0305781307994579232e-1|
|-0.375|1.7969264510985750190e-1|2.1927194805357824279e-1|7.8072805194642175721e-1|
|-0.25|1.8062612864476103931e-1|2.4180592333444175029e-1|7.5819407666555824971e-1|
|-0.125|1.8029165923144016862e-1|2.6437563915587752255e-1|7.3562436084412247745e-1|
|0|1.7885416067524943505e-1|2.8683288012541777457e-1|7.1316711987458222543e-1|
|0.125|1.7647771926844883683e-1|3.0905006637141370265e-1|6.9094993362858629735e-1|
|0.25|1.7331968992724905972e-1|3.3091975892319781116e-1|6.6908024107680218884e-1|
|0.375|1.6952670961396708936e-1|3.5235355101420051309e-1|6.4764644898579948691e-1|
|0.5|1.6523227548265744407e-1|3.7328056247968012295e-1|6.2671943752031987705e-1|
|0.625|1.6055554826785619167e-1|3.9364571141363741810e-1|6.0635428858636258190e-1|
|0.75|1.5560107222369464793e-1|4.1340789653350929690e-1|5.8659210346649070310e-1|
|0.875|1.5045914724271732904e-1|4.3253818780148235207e-1|5.6746181219851764792e-1|
|1|1.4520663709640194253e-1|4.5101809281952585982e-1|5.4898190718047414017e-1|
|1.125|1.3990804417225925227e-1|4.6883794245045029567e-1|5.3116205754954970433e-1|
|1.25|1.3461672245318876186e-1|4.8599542056238026306e-1|5.1400457943761973693e-1|
|1.375|1.2937613544113511623e-1|5.0249424901652591508e-1|4.9750575098347408492e-1|
|1.5|1.2422109407147186097e-1|5.1834302919476012105e-1|4.8165697080523987895e-1|
|1.625|1.1917893186126847083e-1|5.3355423469190897267e-1|4.6644576530809102733e-1|
|1.75|1.1427059137267776398e-1|5.4814334555663845043e-1|4.5185665444336154957e-1|
|1.875|1.0951160844970006377e-1|5.6212811204031342377e-1|4.3787188795968657622e-1|
|2|1.0491298948062606683e-1|5.7552793470311284603e-1|4.2447206529688715397e-1|
|2.125|1.0048198294034004606e-1|5.8836334753367754673e-1|4.1163665246632245327e-1|
|2.25|9.6222750351119111288e-2|6.0065559115706427981e-1|3.9934440884293572019e-1|
|2.375|9.2136944121096479500e-2|6.1242626400693629948e-1|3.8757373599306370051e-1|
|2.5|8.8224200916354986620e-2|6.2369704035487186366e-1|3.7630295964512813634e-1|
|2.625|8.4482559636718838158e-2|6.3448944520421525468e-1|3.6551055479578474532e-1|
|2.75|8.0908812954807221240e-2|6.4482467718710305321e-1|3.5517532281289694678e-1|
|2.875|7.7498800933784133230e-2|6.5472347169815319032e-1|3.4527652830184680967e-1|
|3|7.4247654599601391859e-2|6.6420599752421811067e-1|3.3579400247578188932e-1|
|3.125|7.1149996605791831074e-2|6.7329178116859354034e-1|3.2670821883140645965e-1|
|3.25|6.8200105359711193808e-2|6.8199965391232063128e-1|3.1800034608767936872e-1|
|3.375|6.5392048222494208454e-2|6.9034771740374218477e-1|3.0965228259625781523e-1|
|3.5|6.2719788678318846857e-2|6.9835332422369826739e-1|3.0164667577630173261e-1|
|3.625|6.0177271707562545360e-2|7.0603307044375080265e-1|2.9396692955624919735e-1|
|3.75|5.7758491000638129416e-2|7.1340279768610982924e-1|2.8659720231389017075e-1|
|3.875|5.5457541116776898108e-2|7.2047760261457262074e-1|2.7952239738542737926e-1|
|4|5.3268657223284094017e-2|7.2727185214387559838e-1|2.7272814785612440162e-1|
|4.25|4.9204900077490977132e-2|7.4007262418495588840e-1|2.5992737581504411160e-1|
|4.5|4.5524840154086471489e-2|7.5190626740465383175e-1|2.4809373259534616825e-1|
|4.75|4.2189501453723160889e-2|7.6286376121860206382e-1|2.3713623878139793618e-1|
|5|3.9163419581290406946e-2|7.7302677994283581747e-1|2.2697322005716418253e-1|
|5.25|3.6414570738492001762e-2|7.8246856467781426818e-1|2.1753143532218573182e-1|
|5.5|3.3914194854693572361e-2|7.9125476296790098716e-1|2.0874523703209901284e-1|
|5.75|3.1636561767367110980e-2|7.9944421630620917405e-1|2.0055578369379082594e-1|
|6|2.9558712418470675203e-2|8.0708968553358234182e-1|1.9291031446641765818e-1|
|6.25|2.7660195407525405844e-2|8.1423851062645001755e-1|1.8576148937354998245e-1|
|6.5|2.5922811356501805645e-2|8.2093320540314352240e-1|1.7906679459685647759e-1|
|6.75|2.4330372261263871485e-2|8.2721199008716909778e-1|1.7278800991283090222e-1|
|7|2.2868479523695088460e-2|8.3310926599371992323e-1|1.6689073400628007676e-1|
|7.25|2.1524322111055090777e-2|8.3865603722727131178e-1|1.6134396277272868822e-1|
|7.5|2.0286494878918835976e-2|8.4388028444926438207e-1|1.5611971555073561793e-1|
|7.75|1.9144836246775616554e-2|8.4880729566857517220e-1|1.5119270433142482780e-1|
|8|1.8090283941810259988e-2|8.5345995873929337351e-1|1.4654004126070662649e-1|
|8.5|1.6210994515662290875e-2|8.6202331227050444781e-1|1.3797668772949555219e-1|
|9|1.4593623630742662391e-2|8.6971454657868979660e-1|1.3028545342131020340e-1|
|9.5|1.3194006733673910066e-2|8.7665318131159261240e-1|1.2334681868840738760e-1|
|10|1.1976487388788528718e-2|8.8293886591356384723e-1|1.1706113408643615276e-1|
|10.5|1.0912115437252013003e-2|8.8865516396347839735e-1|1.1134483603652160264e-1|
|11|9.9772518857718462731e-3|8.9387254275217786436e-1|1.0612745724782213564e-1|
|11.5|9.1524856254992540593e-3|8.9865074672038842844e-1|1.0134925327961157156e-1|
|12|8.4217890581505761247e-3|9.0304069162748761805e-1|9.6959308372512381944e-2|
|12.5|7.7718570100181105271e-3|9.0708598436775630050e-1|9.2914015632243699503e-2|
|13|7.1915866461572528046e-3|9.1082414902282322609e-1|8.9175850977176773907e-2|
|13.5|6.6716662460917206733e-3|9.1428762124929801688e-1|8.5712378750701983123e-2|
|14|6.2042483810755441377e-3|9.1750455904064115405e-1|8.2495440959358845950e-2|
|14.5|5.7826888280035436409e-3|9.2049950718907004347e-1|7.9500492810929956528e-2|
|15|5.4013369280103984573e-3|9.2329394458522379855e-1|7.6706055414776201447e-2|
|15.5|5.0553664025156413910e-3|9.2590673721186652774e-1|7.4093262788133472256e-2|
|16|4.7406381433296971811e-3|9.2835451484900244126e-1|7.1645485150997558734e-2|
|17|4.1911372143970710794e-3|9.3281220074559883439e-1|6.7187799254401165610e-2|
|18|3.7296910806918955009e-3|9.3676607926657048988e-1|6.3233920733429510114e-2|
|19|3.3387828254654301003e-3|9.4029505075601315408e-1|5.9704949243986845920e-2|
|20|3.0049793956922566167e-3|9.4346264685125708509e-1|5.6537353148742914908e-2|
|21|2.7178548458224304693e-3|9.4632054452028548747e-1|5.3679455479714512530e-2|
|22|2.4692270065767004073e-3|9.4891117014060104287e-1|5.1088829859398957128e-2|
|23|2.2526085854118386138e-3|9.5126965408765407345e-1|4.8730345912345926547e-2|
|24|2.0628071934785309651e-3|9.5342531530916948121e-1|4.6574684690830518785e-2|
|25|1.8956302704659889306e-3|9.5540280137214727067e-1|4.4597198627852729332e-2|
|26|1.7476648749684945817e-3|9.5722297292736548092e-1|4.2777027072634519077e-2|
|27|1.6161115692700714472e-3|9.5890359644643370439e-1|4.1096403553566295607e-2|
|28|1.4986578444598306452e-3|9.6045989162608323039e-1|3.9540108373916769605e-2|
|29|1.3933807623620016374e-3|9.6190496754858065928e-1|3.8095032451419340717e-2|
|30|1.2986714067753381942e-3|9.6325017291031331633e-1|3.6749827089686683664e-2|
|31|1.2131757709265939191e-3|9.6450537930018013210e-1|3.5494620699819867895e-2|
|32|1.1357481437060201038e-3|9.6567921189509996841e-1|3.4320788104900031593e-2|
|64|2.6977351788249885297e-4|9.8343412102113905964e-1|1.6565878978860940353e-2|
|128|6.4893989614233698971e-5|9.9191112809108069572e-1|8.0888719089193042815e-3|
|256|1.5820674891723428725e-5|9.9601447812693466215e-1|3.9855218730653378497e-3|
|512|3.8948645125312754557e-6|9.9802450037743288081e-1|1.9754996225671191844e-3|
|1024|9.6494644170719151475e-7|9.9901719928318719160e-1|9.8280071681280839681e-4|
|2048|2.3998539451106350460e-7|9.9950999679564168272e-1|4.9000320435831727723e-4|
|4096|5.9820367402545700341e-8|9.9975538804273052358e-1|2.4461195726947641764e-4|
|8192|1.4930623730921403226e-8|9.9987780160058865109e-1|1.2219839941134891231e-4|
|16384|3.7292864082871180172e-9|9.9993893025141979130e-1|6.1069748580208701297e-5|
|32768|9.3186134290455009977e-10|9.9996947312999976526e-1|3.0526870000234731343e-5|
|65536|2.3290290382993096732e-10|9.9998473872684314210e-1|1.5261273156857899227e-5|
|131072|5.8217308264606891035e-11|9.9999236994413684373e-1|7.6300558631562685856e-6|
|262144|1.4553198043127624613e-11|9.9999618512732030472e-1|3.8148726796952767944e-6|
|524288|3.6381487740431616552e-12|9.9999809260499280695e-1|1.9073950071930505131e-6|
|1048576|9.0951714975794989354e-13|9.9999904631345969737e-1|9.5368654030263095324e-7|
|2097152|2.2737663172229661958e-13|9.9999952315962823520e-1|4.7684037176479820788e-7|
|4194304|5.6843807181980884140e-14|9.9999976158057810938e-1|2.3841942189061921489e-7|
|8388608|1.4210905603717844463e-14|9.9999988079048990204e-1|1.1920951009795096744e-7|
|16777216|3.5527203334169665121e-15|9.9999994039529762530e-1|5.9604702374693877753e-8|

input:  
N&#91;PDF&#91;LandauDistribution&#91;0, pi/2&#93;, -22278298125640850406/100000000000000000000&#93;, 42&#93;  
FindRoot&#91;CDF&#91;LandauDistribution&#91;0, pi/2&#93;, x&#93;==0.5, &#123;x, 1.35578&#125;, WorkingPrecision -> 10&#93;

|stat|x|pdf/cdf|
|----|----|----|
|mode|-0.22278298125640850406|pdf:1.806556338205509427830338852686311455672580e-1|
|||cdf:2.46722564066880654991e-1|
|mode+&epsilon;|-0.22278298125640850405|pdf:1.806556338205509427830338852686311455672526e-1|
|mode-&epsilon;|-0.22278298125640850407|pdf:1.806556338205509427830338852686311455672555e-1|
|0.001-quantile|-2.62916563729442102561|pdf:5.53616396793125632751e-3|
|||cdf:1.00000000000000000000e-3|
|0.0025-quantile|-2.44950010103172525499|pdf:1.17219137893221277325e-2|
|||cdf:2.50000000000000000000e-3|
|0.005-quantile|-2.29064564961382595297|pdf:2.02725902569228062119e-2|
|||cdf:5.00000000000000000000e-3|
|0.01-quantile|-2.10489790934939769338|pdf:3.42773294495445546634e-2|
|||cdf:1.00000000000000000000e-2|
|0.025-quantile|-1.79957402870039992314|pdf:6.53597293311529846705e-2|
|||cdf:2.50000000000000000000e-2|
|0.05-quantile|-1.49825415177780273396|pdf:1.00755421921237016806e-1|
|||cdf:5.00000000000000000000e-2|
|0.1-quantile|-1.09225452805484635423|pdf:1.43614595304769939252e-1|
|||cdf:1.00000000000000000000e-1|
|0.15-quantile|-0.77188422256801273027|pdf:1.66651246301470462725e-1|
|||cdf:1.50000000000000000000e-1|
|0.2-quantile|-0.48277711307393456125|pdf:1.77748688795100712727e-1|
|||cdf:2.00000000000000000000e-1|
|0.25-quantile|-0.20464065154575316905|pdf:1.80642733739572559544e-1|
|||cdf:2.50000000000000000000e-1|
|0.3-quantile|0.073877362330763778128|pdf:1.77552824904031234758e-1|
|||cdf:3.00000000000000000000e-1|
|0.35-quantile|0.361135283101730744896|pdf:1.69974460237189067086e-1|
|||cdf:3.50000000000000000000e-1|
|0.4-quantile|0.664768389237266480219|pdf:1.59004839147345548518e-1|
|||cdf:4.00000000000000000000e-1|
|0.45-quantile|0.992995803490664167419|pdf:1.45502720472479260093e-1|
|||cdf:4.50000000000000000000e-1|
|0.5-quantile|1.35578042099080132503|pdf:1.30177124012278500446e-1|
|||cdf:5.00000000000000000000e-1|
|0.55-quantile|1.76629275098246523993|pdf:1.13641554166689275741e-1|
|||1-cdf:4.50000000000000000000e-1|
|0.6-quantile|2.24319477339787288381|pdf:9.64501721395269650776e-2|
|||1-cdf:4.00000000000000000000e-1|
|0.65-quantile|2.81468113996303570812|pdf:7.91241567679137110188e-2|
|||1-cdf:3.50000000000000000000e-1|
|0.7-quantile|3.52636966712139778598|pdf:6.21728308638138898835e-2|
|||1-cdf:3.00000000000000000000e-1|
|0.75-quantile|4.45839461019464834851|pdf:4.61123629197672502396e-2|
|||1-cdf:2.50000000000000000000e-1|
|0.8-quantile|5.76761029823977006164|pdf:3.14839071709654162960e-2|
|||1-cdf:2.00000000000000000000e-1|
|0.85-quantile|7.81274710480049833585|pdf:1.88723009862749446388e-2|
|||1-cdf:1.50000000000000000000e-1|
|0.9-quantile|11.649284684474405569996|pdf:8.92512215537733177903e-3|
|||1-cdf:1.00000000000000000000e-1|
|0.95-quantile|22.450278078872781782888|pdf:2.36806377597673980822e-3|
|||1-cdf:5.00000000000000000000e-2|
|0.975-quantile|43.202525183847707366482|pdf:6.08847503967880454084e-4|
|||1-cdf:2.50000000000000000000e-2|
|0.99-quantile|104.156361812207433543595|pdf:9.89848016724864538104e-5|
|||1-cdf:1.00000000000000000000e-2|
|0.995-quantile|204.862416150123003839536|pdf:2.48740118151505872194e-5|
|||1-cdf:5.00000000000000000000e-3|
|0.9975-quantile|405.562095010331006820385|pdf:6.23431200446070672956e-6|
|||1-cdf:2.50000000000000000000e-3|
|0.999-quantile|1006.482330369225631256342|pdf:9.98998368045158069035e-7|
|||1-cdf:1.00000000000000000000e-3|